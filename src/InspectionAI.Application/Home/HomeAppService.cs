using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using InspectionAI.Home.Dto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Home
{
    public class HomeAppService : IHomeAppService
    {
        private readonly IRepository<Stage.Stage> _stageRepo;
        private readonly IRepository<AssemblyLine.AssemblyLine> _assemblyLineRepo;
        private readonly IRepository<Defects.Defects> _defectsRepo;
        private readonly IRepository<StageDefects.StageDefects> _stageDefectsRepo;
        private readonly IRepository<AssemblyDetection.AssemblyDetection> _assemblyDetectionRepo;
        private readonly IRepository<AssemblyDefects.AssemblyDefects> _assemblyDefectsRepo;
        private readonly IRepository<Product.Product> _productRepo;

        public HomeAppService(IRepository<Stage.Stage> stageRepo,
                              IRepository<Product.Product> productRepo,
                              IRepository<AssemblyLine.AssemblyLine> assemblyLineRepo,
                              IRepository<Defects.Defects> defectsRepo,
                              IRepository<StageDefects.StageDefects> stageDefectsRepo,
                              IRepository<AssemblyDetection.AssemblyDetection> assemblyDetectionRepo,
                              IRepository<AssemblyDefects.AssemblyDefects> assemblyDefectsRepo)
        {
            _stageRepo = stageRepo;
            _productRepo = productRepo;
            _assemblyLineRepo = assemblyLineRepo;
            _defectsRepo = defectsRepo;
            _stageDefectsRepo = stageDefectsRepo;
            _assemblyDetectionRepo = assemblyDetectionRepo;
            _assemblyDefectsRepo = assemblyDefectsRepo;
        }

        public void FillData(int step, int perDayCount, DateTime from, DateTime to)
        {
            var assemData =  _assemblyLineRepo.GetAllList();
            var stageDefects = _stageDefectsRepo.GetAllList();
            for (DateTime d = from; d <= to; d = d.AddDays(1))
            {
                for (int i = 0; i < perDayCount; i++)
                {
                    Random rnd = new Random();
                    var assem = assemData.ElementAt(rnd.Next(assemData.Count));
                    var productId = assem.ProductId;
                    var stageId = assem.StageId;
                    var assemId = assem.Id;
                    var defectsIds = stageDefects.Where(x => x.ProductId == productId && x.StageId == stageId).Select(x => x.DefectId).ToList();
                    var dCount = rnd.Next(10) % 2;
                    AssemblyDetection.AssemblyDetection detection = new()
                    {
                        StageId = stageId,
                        ProductId = productId,
                        AssemblyLineId = assemId,
                        DetectionTime = d,
                        DefectsCount = dCount
                    };
                    var detectionId = _assemblyDetectionRepo.InsertAndGetId(detection);
                    for( int j = 0; j < dCount; j++)
                    {
                        AssemblyDefects.AssemblyDefects assemblyDefect = new()
                        {
                            AssemblyDetectionId = detectionId,
                            Confidence = 9,
                            DefectId = defectsIds.ElementAt(rnd.Next(defectsIds.Count)),
                            DetectionTime = d,
                            StageId = stageId
                        };
                        _assemblyDefectsRepo.Insert(assemblyDefect);
                    }
                }
            }
        }

        public async Task<GeneralInsightsDto> GetGeneralInsightsAsync(string duration,int productId, int stageId, int type)
        {
            if (duration.Equals("Weekly"))
            {
                return await WeeklyInsightsAsync(productId, stageId, type);
            }
            else if (duration.Equals("Monthly"))
            {
                return await MonthlyInsightsAsync(productId, stageId, type);
            }
            else if (duration.Equals("Yearly"))
            {
                return await YearlyInsightsAsync(productId, stageId, type);
            }
            return new();
        }

        public async Task<RevenueLossDto> GetProductRevenueLoss(string duration)
        {
            if (duration.Equals("Weekly"))
            {
                return await ProductRevenueLossWeeklyMonthly(duration);
            }
            else if (duration.Equals("Monthly"))
            {
                return await ProductRevenueLossWeeklyMonthly(duration);
            }
            else if (duration.Equals("Yearly"))
            {
                return await ProductRevenueLossYearly(duration);
            }
            return new();
        }

        private async Task<RevenueLossDto> ProductRevenueLossWeeklyMonthly(string duration)
        {
            RevenueLossDto revenueLoss = new();
            var products = await _productRepo.GetAllListAsync();
            List<AssemblyDetection.AssemblyDetection> detections = new();
            if (duration.Equals("Weekly"))
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-6));
            else if (duration.Equals("Monthly"))
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-29));
            revenueLoss.Labels = detections.GroupBy(x => x.DetectionTime.Date).Select(x => x.Key).ToList();
            //revenueLoss.Labels = detections.Select(x => x.DetectionTime.Date).ToList();
            revenueLoss.All = new(new double[revenueLoss.Labels.Count]);
            var defectives = detections.GroupBy(x => x.DetectionTime.Date);

            revenueLoss.Data = new(new RevenueLossDataDto[products.Count]);
            for (int i = 0; i < products.Count; i++)
            {
                revenueLoss.Data[i] = new RevenueLossDataDto();
                revenueLoss.Data[i].Id = products[i].Id;
                revenueLoss.Data[i].Name = products[i].Name;
                revenueLoss.Data[i].Data = new(new double[revenueLoss.All.Count]);
            }
            var stages = await _stageRepo.GetAllListAsync();
            foreach (var defectData in revenueLoss.Data)
            {
                var data = detections.Where(x => x.ProductId == defectData.Id)
                    .GroupBy(x => x.DetectionTime.Date).ToList();

                data.ForEach(x =>
                {
                    int index = revenueLoss.Labels.FindIndex(y => y.Date == x.Key);
                    double loss = 0;
                    foreach( var group in x)
                    {
                        loss += stages.Where(x => x.Id == group.StageId && x.ProductId == group.ProductId).Select(y => y.Cost).ElementAt(0);
                    }
                    if (index != -1)
                    {
                        defectData.Data[index] = loss;
                        revenueLoss.All[index] += loss;
                    }
                });
            }
            return revenueLoss;

        }

        private async Task<RevenueLossDto> ProductRevenueLossYearly(string duration)
        {
            RevenueLossDto revenueLoss = new();
            var products = await _productRepo.GetAllListAsync();
            var detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddMonths(-11));
            revenueLoss.Labels = detections.GroupBy(x => x.DetectionTime.Month).Select(x => x.ElementAt(0).DetectionTime.Date).ToList();
            revenueLoss.All = new(new double[revenueLoss.Labels.Count]);
            var defectives = detections.GroupBy(x => x.DetectionTime.Month);

            revenueLoss.Data = new(new RevenueLossDataDto[products.Count]);
            for (int i = 0; i < products.Count; i++)
            {
                revenueLoss.Data[i] = new RevenueLossDataDto();
                revenueLoss.Data[i].Id = products[i].Id;
                revenueLoss.Data[i].Name = products[i].Name;
                revenueLoss.Data[i].Data = new(new double[revenueLoss.All.Count]);
            }
            var stages = await _stageRepo.GetAllListAsync();
            foreach (var defectData in revenueLoss.Data)
            {
                var data = detections.Where(x => x.ProductId == defectData.Id)
                    .GroupBy(x => x.DetectionTime.Month).ToList();

                data.ForEach(x =>
                {
                    int index = revenueLoss.Labels.FindIndex(y => y.Month == x.Key);
                    double loss = 0;
                    foreach (var group in x)
                    {
                        loss += stages.Where(x => x.Id == group.StageId && x.ProductId == group.ProductId).Select(y => y.Cost).ElementAt(0);
                    }
                    if (index != -1)
                    {
                        defectData.Data[index] = loss;
                        revenueLoss.All[index] += loss;
                    }
                });
            }
            return revenueLoss;

        }

        public async Task<ProductDefectRatioDto> GetProductDefectRatio(string duration)
        {
            ProductDefectRatioDto insightsDto = new();
            var products = await _productRepo.GetAllListAsync();
            List<AssemblyDetection.AssemblyDetection> detections = new();
            if (duration.Equals("Weekly"))
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-6));
            else if (duration.Equals("Monthly"))
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-29));
            else if (duration.Equals("Yearly"))
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.DetectionTime.Date >= DateTime.Now.Date.AddMonths(-11));
            var goods = detections.Where(x => x.DefectsCount == 0).GroupBy(x => x.ProductId);
            var defectives = detections.Where(x => x.DefectsCount > 0).GroupBy(x => x.ProductId);
            insightsDto.Name = products.Select(x => x.Name).ToList();
            insightsDto.Ids = products.Select(x => x.Id).ToList();
            insightsDto.Good = new(new int[products.Count()]);
            insightsDto.Defects = new(new int[products.Count()]);
            foreach (var good in goods)
            {
                int index = insightsDto.Ids.FindIndex(x => x == good.Key);
                if (index != -1)
                    insightsDto.Good[index] = good.Count();
            }
            foreach (var defect in defectives)
            {
                int index = insightsDto.Ids.FindIndex(x => x == defect.Key);
                if (index != -1)
                    insightsDto.Defects[index] = defect.Count();
            }
            return insightsDto;
        }

        public async Task<DefectiveRatio> GetDefectiveProducts(string duration)
        {
            DefectiveRatio defectiveRatio = new();
            var products = await _productRepo.GetAllListAsync();
            List<AssemblyDetection.AssemblyDetection> detections = new();
            if (duration.Equals("Weekly"))
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-6));
            else if (duration.Equals("Monthly"))
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-29));
            else if (duration.Equals("Yearly"))
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddMonths(-11));
            var defectives = detections.GroupBy(x => x.ProductId);
            defectiveRatio.Names = products.Select(x => x.Name).ToList();
            List<int> ids = products.Select(x => x.Id).ToList();
            defectiveRatio.Count = new(new int[defectiveRatio.Names.Count]);
            foreach (var defect in defectives)
            {
                int index = ids.FindIndex(x => x == defect.Key);
                if (index != -1)
                    defectiveRatio.Count[index] = defect.Count();
            }
            return defectiveRatio;
        }

        public async Task<ProductDefectTrendDto> GetProductDefectTrend(string duration)
        {
            {
                if (duration.Equals("Weekly"))
                {
                    return await ProductDefectTrendWeeklyMonthly(duration);
                }
                else if (duration.Equals("Monthly"))
                {
                    return await ProductDefectTrendWeeklyMonthly(duration);
                }
                else if (duration.Equals("Yearly"))
                {
                    return await ProductDefectTrendYearly(duration);
                }
                return new();
            }
        }

        private async Task<ProductDefectTrendDto> ProductDefectTrendWeeklyMonthly(string duration)
        {
            ProductDefectTrendDto defectTrend = new();
            var products = await _productRepo.GetAllListAsync();
            List<AssemblyDetection.AssemblyDetection> detections = new();
            if (duration.Equals("Weekly"))
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-6));
            else if (duration.Equals("Monthly"))
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-29));
            defectTrend.Labels = detections.GroupBy(x => x.DetectionTime.Date).Select(x => x.Key).ToList();
            //defectTrend.Labels = detections.Select(x => x.DetectionTime.Date).ToList();
            defectTrend.All = new(new int[defectTrend.Labels.Count]);
            var defectives = detections.GroupBy(x => x.DetectionTime.Date);

            foreach (var defect in defectives)
            {
                int index = defectTrend.Labels.FindIndex(x => x.Date == defect.Key);
                if (index != -1)
                    defectTrend.All[index] = defect.Count();
            }
            defectTrend.Data = new(new ProductDefectTrendDataDto[products.Count]);
            for (int i = 0; i < products.Count; i++)
            {
                defectTrend.Data[i] = new ProductDefectTrendDataDto();
                defectTrend.Data[i].Id = products[i].Id;
                defectTrend.Data[i].Name = products[i].Name;
                defectTrend.Data[i].Data = new(new int[defectTrend.All.Count]);
            }

            foreach (var defectData in defectTrend.Data)
            {
                var data = detections.Where(x => x.ProductId == defectData.Id)
                    .GroupBy(x => x.DetectionTime.Date)
                    .Select(c => new
                    {
                        count = c.Count(),
                        date = c.Key,
                    }).ToList();

                data.ForEach(x =>
                {
                    int index = defectTrend.Labels.FindIndex(y => y.Date == x.date);
                    if (index != -1)
                        defectData.Data[index] = x.count;
                });
            }
            return defectTrend;
        }

        private async Task<ProductDefectTrendDto> ProductDefectTrendYearly(string duration)
        {
            ProductDefectTrendDto defectTrend = new();
            var products = await _productRepo.GetAllListAsync();
            List<AssemblyDetection.AssemblyDetection> detections = new();
            detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddMonths(-11));
            defectTrend.Labels = detections.GroupBy(x => x.DetectionTime.Month).Select(x => x.ElementAt(0).DetectionTime.Date).ToList();
            //defectTrend.Labels = detections.Select(x => x.DetectionTime.Date).ToList();
            defectTrend.All = new(new int[defectTrend.Labels.Count]);
            var defectives = detections.GroupBy(x => x.DetectionTime.Month);

            foreach (var defect in defectives)
            {
                int index = defectTrend.Labels.FindIndex(x => x.Date == defect.ElementAt(0).DetectionTime.Date);
                if (index != -1)
                    defectTrend.All[index] = defect.Count();
            }
            defectTrend.Data = new(new ProductDefectTrendDataDto[products.Count]);
            for (int i = 0; i < products.Count; i++)
            {
                defectTrend.Data[i] = new ProductDefectTrendDataDto();
                defectTrend.Data[i].Id = products[i].Id;
                defectTrend.Data[i].Name = products[i].Name;
                defectTrend.Data[i].Data = new(new int[defectTrend.All.Count]);
            }

            foreach (var defectData in defectTrend.Data)
            {
                var data = detections.Where(x => x.ProductId == defectData.Id)
                    .GroupBy(x => x.DetectionTime.Month).ToList();

                data.ForEach(x =>
                {
                    int index = defectTrend.Labels.FindIndex(y => y.Date == x.ElementAt(0).DetectionTime.Date);
                    if (index != -1)
                        defectData.Data[index] = x.Count();
                });
            }
            return defectTrend;
        }

        public async Task<DefectTrendDto> GetDefectTrendAsync(string duration, int productId, int stageId, int type)
        {
            if (duration.Equals("Weekly"))
            {
                return await WeeklyDefectTrend(productId, stageId, type);
            }
            else if (duration.Equals("Monthly"))
            {
                return await MonthlyDefectTrend(productId, stageId, type);
            }
            else if (duration.Equals("Yearly"))
            {
                return await YearlyDefectTrend(productId, stageId, type);
            }
            return new();
        }

        public async Task<RevenueLossDto> GetRevenueLossAsync(string duration, int productId, int stageId)
        {
            if (duration.Equals("Weekly"))
            {
                return await WeeklyRevenueLoss(productId, stageId);
            }
            else if (duration.Equals("Monthly"))
            {
                return await MonthlyRevenueLoss(productId, stageId);
            }
            else if (duration.Equals("Yearly"))
            {
                return await YearlyRevenueLoss(productId, stageId);
            }
            return new();
        }

        public async Task<Dto.AssemblyDefects> GetAssemblyDefectsAsync(string duration, int productId, int stageId)
        {
            if (duration.Equals("Weekly"))
            {
                return await WeeklyAssemblyDefects(productId, stageId);
            }
            else if (duration.Equals("Monthly"))
            {
                return await MonthlyAssemblyDefects(productId, stageId);
            }
            else if (duration.Equals("Yearly"))
            {
                return await YearlyAssemblyDefects(productId, stageId);
            }
            return new();
        }



        public async Task<DefectiveRatio> GetDefectiveRatio(string duration, int productId, int stageId, int type)
        {
            DefectiveRatio defectiveRatio = new();
            List<AssemblyDefects.AssemblyDefects> defectsData = new List<AssemblyDefects.AssemblyDefects>();
            List<Defects.Defects> defects = await _defectsRepo.GetAllListAsync();
            if (type == (byte)TypeEnum.STAGE)
            {
                if (duration.Equals("Weekly"))
                    defectsData = await _assemblyDefectsRepo.GetAllListAsync(x => x.StageId == stageId && x.DetectionTime >= DateTime.Now.Date.AddDays(-7));
                else if (duration.Equals("Monthly"))
                    defectsData = await _assemblyDefectsRepo.GetAllListAsync(x => x.StageId == stageId && x.DetectionTime >= DateTime.Now.Date.AddDays(-30));
                else if (duration.Equals("Yearly"))
                    defectsData = await _assemblyDefectsRepo.GetAllListAsync(x => x.StageId == stageId && x.DetectionTime >= DateTime.Now.Date.AddMonths(-11));
            }
            else if (type == (byte)TypeEnum.FULL)
            {
                if (duration.Equals("Weekly"))
                    defectsData = await _assemblyDefectsRepo.GetAllListAsync(x => x.DetectionTime >= DateTime.Now.Date.AddDays(-7));
                else if (duration.Equals("Monthly"))
                    defectsData = await _assemblyDefectsRepo.GetAllListAsync(x => x.DetectionTime >= DateTime.Now.Date.AddDays(-30));
                else if (duration.Equals("Yearly"))
                    defectsData = await _assemblyDefectsRepo.GetAllListAsync(x => x.DetectionTime >= DateTime.Now.Date.AddMonths(-11));
            }

            var group = defectsData.GroupBy(x => x.DefectId);
            defectiveRatio.Names = new();
            defectiveRatio.Count = new();
            foreach(var groupData in group)
            {
                defectiveRatio.Names.Add(defects.Find(x => x.Id == groupData.Key).Name);
                defectiveRatio.Count.Add(groupData.Count());
            }
            return defectiveRatio;
        }

        private async Task<Dto.AssemblyDefects> WeeklyAssemblyDefects(int productId, int stageId)
        {
            Dto.AssemblyDefects assemblyDefects = new();
            var assemblies = await _assemblyLineRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId);
            assemblyDefects.AssemblyNames = assemblies.Select(x => x.Name).ToList();
            assemblyDefects.AssemblyId = assemblies.Select(x => x.Id).ToList();
            var defects = await _assemblyDetectionRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId && x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-6).Date);
            var defectList = defects.GroupBy(x => x.AssemblyLineId);
            assemblyDefects.AssemblyDefectsCount = new(new int[assemblyDefects.AssemblyId.Count]);
            foreach (var group in defectList)
            {
                var defectsCount = 0;
                foreach (var d in group)
                {
                    defectsCount += d.DefectsCount;
                }
                int index = assemblyDefects.AssemblyId.FindIndex(y => y == group.Key);
                if (index != -1)
                    assemblyDefects.AssemblyDefectsCount[index] = defectsCount;
            }
            return assemblyDefects;
        }

        private async Task<Dto.AssemblyDefects> MonthlyAssemblyDefects(int productId, int stageId)
        {
            Dto.AssemblyDefects assemblyDefects = new();
            var assemblies = await _assemblyLineRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId);
            assemblyDefects.AssemblyNames = assemblies.Select(x => x.Name).ToList();
            assemblyDefects.AssemblyId = assemblies.Select(x => x.Id).ToList();
            var defects = await _assemblyDetectionRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId && x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-29).Date);
            var defectList = defects.GroupBy(x => x.AssemblyLineId);
            assemblyDefects.AssemblyDefectsCount = new(new int[assemblyDefects.AssemblyId.Count]);
            foreach (var group in defectList)
            {
                var defectsCount = 0;
                foreach (var d in group)
                {
                    defectsCount += d.DefectsCount;
                }
                int index = assemblyDefects.AssemblyId.FindIndex(y => y == group.Key);
                if (index != -1)
                    assemblyDefects.AssemblyDefectsCount[index] = defectsCount;
            }
            return assemblyDefects;
        }

        private async Task<Dto.AssemblyDefects> YearlyAssemblyDefects(int productId, int stageId)
        {
            Dto.AssemblyDefects assemblyDefects = new();
            var assemblies = await _assemblyLineRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId);
            assemblyDefects.AssemblyNames = assemblies.Select(x => x.Name).ToList();
            assemblyDefects.AssemblyId = assemblies.Select(x => x.Id).ToList();
            var defects = await _assemblyDetectionRepo.GetAllListAsync(x =>  x.ProductId == productId && x.StageId == stageId && x.DefectsCount > 0 && x.DetectionTime >= DateTime.Now.Date.AddMonths(-11));
            var defectList = defects.GroupBy(x => x.AssemblyLineId);
            assemblyDefects.AssemblyDefectsCount = new(new int[assemblyDefects.AssemblyId.Count]);
            foreach (var group in defectList)
            {
                var defectsCount = 0;
                foreach (var d in group)
                {
                    defectsCount += d.DefectsCount;
                }
                int index = assemblyDefects.AssemblyId.FindIndex(y => y == group.Key);
                if (index != -1)
                    assemblyDefects.AssemblyDefectsCount[index] = defectsCount;
            }
            return assemblyDefects;
        }

        private async Task<RevenueLossDto> WeeklyRevenueLoss(int productId, int stageId)
        {
            RevenueLossDto revenueLoss = new();
            var detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId && x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-6));
            var detectionList = detections.GroupBy(x => x.DetectionTime.Date).ToList();

            var stage = await _stageRepo.GetAsync(stageId);

            revenueLoss.Labels = detectionList.Select(x => x.Key.Date).ToList();
            revenueLoss.All = new(new double[revenueLoss.Labels.Count]);
            foreach (var group in detectionList)
            {
                int index = revenueLoss.Labels.FindIndex(x => x.Date == group.Key);
                if (index != -1)
                    revenueLoss.All[index] = group.Count() * stage.Cost;
            }

            var assemblyLines = await _assemblyLineRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId);
            revenueLoss.Data = new(new RevenueLossDataDto[assemblyLines.Count]);
            for (int i = 0; i < assemblyLines.Count; i++)
            {
                revenueLoss.Data[i] = new RevenueLossDataDto();
                revenueLoss.Data[i].Id = assemblyLines[i].Id;
                revenueLoss.Data[i].Name = assemblyLines[i].Name;
                revenueLoss.Data[i].Data = new(new double[revenueLoss.All.Count]);
            }
           // var detectionDefects = await _assemblyDetectionRepo.GetAllListAsync(x => x.StageId == stageId && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-6));
            foreach (var revenueLossData in revenueLoss.Data)
            {
                var data = detections.Where(x => x.AssemblyLineId == revenueLossData.Id)
                    .GroupBy(x => x.DetectionTime.Date).ToList();

                data.ForEach(x =>
                {
                    int index = revenueLoss.Labels.FindIndex(y => y.Date == x.Key);
                    if (index != -1)
                        revenueLossData.Data[index] = x.Count() * stage.Cost;
                });
            }
            return revenueLoss;
        }

        private async Task<RevenueLossDto> MonthlyRevenueLoss(int productId, int stageId)
        {
            RevenueLossDto revenueLoss = new();
            var detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId && x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-29));
            var detectionList = detections.GroupBy(x => x.DetectionTime.Date).ToList();

            var stage = await _stageRepo.GetAsync(stageId);

            revenueLoss.Labels = detectionList.Select(x => x.Key.Date).ToList();
            revenueLoss.All = new(new double[revenueLoss.Labels.Count]);
            foreach (var group in detectionList)
            {
                int index = revenueLoss.Labels.FindIndex(x => x.Date == group.Key);
                if (index != -1)
                    revenueLoss.All[index] = group.Count() * stage.Cost;
            }

            var assemblyLines = await _assemblyLineRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId);
            revenueLoss.Data = new(new RevenueLossDataDto[assemblyLines.Count]);
            for (int i = 0; i < assemblyLines.Count; i++)
            {
                revenueLoss.Data[i] = new RevenueLossDataDto();
                revenueLoss.Data[i].Id = assemblyLines[i].Id;
                revenueLoss.Data[i].Name = assemblyLines[i].Name;
                revenueLoss.Data[i].Data = new(new double[revenueLoss.All.Count]);
            }
            //var detectionDefects = await _assemblyDetectionRepo.GetAllListAsync(x => x.StageId == stageId && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-29));
            foreach (var revenueLossData in revenueLoss.Data)
            {
                var data = detections.Where(x => x.AssemblyLineId == revenueLossData.Id)
                    .GroupBy(x => x.DetectionTime.Date).ToList();

                data.ForEach(x =>
                {
                    int index = revenueLoss.Labels.FindIndex(y => y.Date == x.Key);
                    if (index != -1)
                        revenueLossData.Data[index] = x.Count() * stage.Cost;
                });
            }
            return revenueLoss;
        }

        private async Task<RevenueLossDto> YearlyRevenueLoss(int productId, int stageId)
        {
            RevenueLossDto revenueLoss = new();
            var detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId && x.DefectsCount > 0 && x.DetectionTime >= DateTime.Now.Date.AddMonths(-11));
            var detectionList = detections.GroupBy(x => x.DetectionTime.Month).ToList();

            var stage = await _stageRepo.GetAsync(stageId);

            revenueLoss.Labels = detectionList.Select(x => x.ElementAt(0).DetectionTime.Date).ToList();
            revenueLoss.All = new(new double[revenueLoss.Labels.Count]);

            foreach (var group in detectionList)
            {
                int index = revenueLoss.Labels.FindIndex(x => x == group.ElementAt(0).DetectionTime.Date);
                if (index != -1)
                    revenueLoss.All[index] = group.Count() * stage.Cost;
            }

            var assemblyLines = await _assemblyLineRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId);
            revenueLoss.Data = new(new RevenueLossDataDto[assemblyLines.Count]);
            for (int i = 0; i < assemblyLines.Count; i++)
            {
                revenueLoss.Data[i] = new RevenueLossDataDto();
                revenueLoss.Data[i].Id = assemblyLines[i].Id;
                revenueLoss.Data[i].Name = assemblyLines[i].Name;
                revenueLoss.Data[i].Data = new(new double[revenueLoss.All.Count]);
            }
            //var detectionDefects = await _assemblyDetectionRepo.GetAllListAsync(x => x.StageId == stageId && x.DetectionTime.Date >= DateTime.Now.Date.AddMonths(-11));
            foreach (var revenueLossData in revenueLoss.Data)
            {
                var data = detections.Where(x => x.AssemblyLineId == revenueLossData.Id)
                    .GroupBy(x => x.DetectionTime.Month).ToList();

                data.ForEach(x =>
                {
                    int index = revenueLoss.Labels.FindIndex(y => y == x.ElementAt(0).DetectionTime.Date);
                    if (index != -1)
                        revenueLossData.Data[index] = x.Count() * stage.Cost;
                });
            }
            return revenueLoss;
        }

        private async Task<DefectTrendDto> WeeklyDefectTrend(int productId, int stageId, int type)
        {
            DefectTrendDto defectTrend = new();
            List<AssemblyDetection.AssemblyDetection> detections = new();
            if (type == (byte)TypeEnum.STAGE)
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId && x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-6));
            else if (type == (byte)TypeEnum.FULL)
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-6));
           
            var detectionList = detections.GroupBy(x => x.DetectionTime.Date).ToList();

            defectTrend.Labels = detectionList.Select(x => x.Key.Date).ToList();
            defectTrend.All = new(new int[defectTrend.Labels.Count]);
            foreach (var defect in detectionList)
            {
                int count = 0;
                foreach(var d in defect)
                {
                    count += d.DefectsCount;
                }
                int index = defectTrend.Labels.FindIndex(x => x.Date == defect.Key);
                if (index != -1)
                    defectTrend.All[index] = count;
            }

            if (type == (byte)TypeEnum.STAGE)
            {
                var stageDefects = _stageDefectsRepo.GetAllIncluding(x => x.Defects).Where(x => x.ProductId == productId && x.StageId == stageId).ToList();
                defectTrend.Data = new(new DefectTrendDataDto[stageDefects.Count]);
                for (int i = 0; i < stageDefects.Count; i++)
                {
                    defectTrend.Data[i] = new DefectTrendDataDto();
                    defectTrend.Data[i].DefectId = stageDefects[i].DefectId;
                    defectTrend.Data[i].Name = stageDefects[i].Defects.Name;
                    defectTrend.Data[i].Data = new(new int[defectTrend.All.Count]);
                }
            }
            else if (type == (byte)TypeEnum.FULL)
            {
                var products = await _productRepo.GetAllListAsync();
                defectTrend.Data = new(new DefectTrendDataDto[products.Count]);
                for (int i = 0; i < products.Count; i++)
                {
                    defectTrend.Data[i] = new DefectTrendDataDto();
                    defectTrend.Data[i].DefectId = products[i].Id;
                    defectTrend.Data[i].Name = products[i].Name;
                    defectTrend.Data[i].Data = new(new int[defectTrend.All.Count]);
                }
            }
            List<AssemblyDefects.AssemblyDefects> detectionDefects = new();
            if (type == (byte)TypeEnum.STAGE)
            {
                detectionDefects = await _assemblyDefectsRepo.GetAllListAsync(x => x.StageId == stageId && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-6));
            }
            else if (type == (byte)TypeEnum.FULL)
            {
                detectionDefects = await _assemblyDefectsRepo.GetAllListAsync(x => x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-6));
            }
            foreach (var defectData in defectTrend.Data)
            {
                var data = detectionDefects.Where(x => x.DefectId == defectData.DefectId)
                    .GroupBy(x => x.DetectionTime.Date)
                    .Select(c => new
                    {
                        count = c.Count(),
                        date = c.Key,
                    }).ToList();
                
                data.ForEach(x =>
                {
                    int index = defectTrend.Labels.FindIndex(y => y.Date == x.date);
                    if (index != -1)
                        defectData.Data[index] = x.count;
                });
            }
            return defectTrend;
        }

        private async Task<DefectTrendDto> MonthlyDefectTrend(int productId, int stageId, int type)
        {
            DefectTrendDto defectTrend = new();
            List<AssemblyDetection.AssemblyDetection> detections = new();
            if (type == (byte)TypeEnum.STAGE)
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId && x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-29));
            else if (type == (byte)TypeEnum.FULL)
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-29));

            var detectionList = detections.GroupBy(x => x.DetectionTime.Date).ToList();

            defectTrend.Labels = detectionList.Select(x => x.Key.Date).ToList();
            defectTrend.All = new(new int[defectTrend.Labels.Count]);
            foreach (var defect in detectionList)
            {
                int count = 0;
                foreach (var d in defect)
                {
                    count += d.DefectsCount;
                }
                int index = defectTrend.Labels.FindIndex(x => x.Date == defect.Key);
                if (index != -1)
                    defectTrend.All[index] = count;
            }

            if (type == (byte)TypeEnum.STAGE)
            {
                var stageDefects = _stageDefectsRepo.GetAllIncluding(x => x.Defects).Where(x => x.ProductId == productId && x.StageId == stageId).ToList();
                defectTrend.Data = new(new DefectTrendDataDto[stageDefects.Count]);
                for (int i = 0; i < stageDefects.Count; i++)
                {
                    defectTrend.Data[i] = new DefectTrendDataDto();
                    defectTrend.Data[i].DefectId = stageDefects[i].DefectId;
                    defectTrend.Data[i].Name = stageDefects[i].Defects.Name;
                    defectTrend.Data[i].Data = new(new int[defectTrend.All.Count]);
                }
            }
            else if (type == (byte)TypeEnum.FULL)
            {
                var products = await _productRepo.GetAllListAsync();
                defectTrend.Data = new(new DefectTrendDataDto[products.Count]);
                for (int i = 0; i < products.Count; i++)
                {
                    defectTrend.Data[i] = new DefectTrendDataDto();
                    defectTrend.Data[i].DefectId = products[i].Id;
                    defectTrend.Data[i].Name = products[i].Name;
                    defectTrend.Data[i].Data = new(new int[defectTrend.All.Count]);
                }
            }
            List<AssemblyDefects.AssemblyDefects> detectionDefects = new();
            if (type == (byte)TypeEnum.STAGE)
            {
                detectionDefects = await _assemblyDefectsRepo.GetAllListAsync(x => x.StageId == stageId && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-29));
            }
            else if (type == (byte)TypeEnum.FULL)
            {
                detectionDefects = await _assemblyDefectsRepo.GetAllListAsync(x => x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-29));
            }
            foreach (var defectData in defectTrend.Data)
            {
                var data = detectionDefects.Where(x => x.DefectId == defectData.DefectId)
                    .GroupBy(x => x.DetectionTime.Date)
                    .Select(c => new
                    {
                        count = c.Count(),
                        date = c.Key,
                    }).ToList();

                data.ForEach(x =>
                {
                    int index = defectTrend.Labels.FindIndex(y => y.Date == x.date);
                    if (index != -1)
                        defectData.Data[index] = x.count;
                });
            }
            return defectTrend;
        }

        private async Task<DefectTrendDto> YearlyDefectTrend(int productId, int stageId, int type)
        {
            DefectTrendDto defectTrend = new();
            List<AssemblyDetection.AssemblyDetection> detections = new();
            if (type == (byte)TypeEnum.STAGE)
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId && x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddMonths(-11));
            else if (type == (byte)TypeEnum.FULL)
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddMonths(-11));

            var detectionList = detections.GroupBy(x => x.DetectionTime.Month).ToList();

            defectTrend.Labels = detectionList.Select(x => x.ElementAt(0).DetectionTime.Date).ToList();
            defectTrend.All = new(new int[defectTrend.Labels.Count]);
            foreach (var defect in detectionList)
            {
                int count = 0;
                foreach (var d in defect)
                {
                    count += d.DefectsCount;
                }
                int index = defectTrend.Labels.FindIndex(x => x == defect.ElementAt(0).DetectionTime.Date);
                if (index != -1)
                    defectTrend.All[index] = count;
            }

            if (type == (byte)TypeEnum.STAGE)
            {
                var stageDefects = _stageDefectsRepo.GetAllIncluding(x => x.Defects).Where(x => x.ProductId == productId && x.StageId == stageId).ToList();
                defectTrend.Data = new(new DefectTrendDataDto[stageDefects.Count]);
                for (int i = 0; i < stageDefects.Count; i++)
                {
                    defectTrend.Data[i] = new DefectTrendDataDto();
                    defectTrend.Data[i].DefectId = stageDefects[i].DefectId;
                    defectTrend.Data[i].Name = stageDefects[i].Defects.Name;
                    defectTrend.Data[i].Data = new(new int[defectTrend.All.Count]);
                }
            }
            else if (type == (byte)TypeEnum.FULL)
            {
                var products = await _productRepo.GetAllListAsync();
                defectTrend.Data = new(new DefectTrendDataDto[products.Count]);
                for (int i = 0; i < products.Count; i++)
                {
                    defectTrend.Data[i] = new DefectTrendDataDto();
                    defectTrend.Data[i].DefectId = products[i].Id;
                    defectTrend.Data[i].Name = products[i].Name;
                    defectTrend.Data[i].Data = new(new int[defectTrend.All.Count]);
                }
            }
            List<AssemblyDefects.AssemblyDefects> detectionDefects = new();
            if (type == (byte)TypeEnum.STAGE)
            {
                detectionDefects = await _assemblyDefectsRepo.GetAllListAsync(x => x.StageId == stageId && x.DetectionTime.Date >= DateTime.Now.Date.AddMonths(-11));
            }
            else if (type == (byte)TypeEnum.FULL)
            {
                detectionDefects = await _assemblyDefectsRepo.GetAllListAsync(x => x.DetectionTime.Date >= DateTime.Now.Date.AddMonths(-11));
            }
            foreach (var defectData in defectTrend.Data)
            {
                var data = detectionDefects.Where(x => x.DefectId == defectData.DefectId)
                    .GroupBy(x => x.DetectionTime.Month).ToList();

                data.ForEach(x =>
                {
                    int index = defectTrend.Labels.FindIndex(y => y.Date == x.ElementAt(0).DetectionTime.Date);
                    if (index != -1)
                        defectData.Data[index] = x.Count();
                });
            }
            return defectTrend;
        }

        private async Task<GeneralInsightsDto> WeeklyInsightsAsync(int productId, int stageId, int type)
        {
            GeneralInsightsDto generalInsightsDto = new();
            List<AssemblyDetection.AssemblyDetection> detections = new();
            if (type == (byte)TypeEnum.FULL)
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-6));
            else if (type == (byte)TypeEnum.STAGE)
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-6));
            generalInsightsDto.TotalDetections = detections.Count;
            generalInsightsDto.TotalGood = detections.Where(x => x.DefectsCount == 0).Count();
            generalInsightsDto.TotalDefects = detections.Where(x => x.DefectsCount > 0).Count();
            var detectionList = detections.GroupBy(x => x.DetectionTime.Date).Select(c => new
            {
                date = c.Key,
                count = c.Count()
            });
            
            generalInsightsDto.Labels = detectionList.Select(x => x.date).ToList();
            generalInsightsDto.Detections = detectionList.Select(x => x.count).ToList();

            generalInsightsDto.Defects = new(new int[generalInsightsDto.Detections.Count]);
            generalInsightsDto.Good = new(new int[generalInsightsDto.Detections.Count]);

            var goodList = detections.Where(x => x.DefectsCount == 0).GroupBy(x => x.DetectionTime.Date).Select(c => new
            {
                date = c.Key,
                count = c.Count()
            });
            foreach(var good in goodList)
            {
                int index = generalInsightsDto.Labels.FindIndex(x => x.Equals(good.date));
                if (index != -1)
                    generalInsightsDto.Good[index] = good.count;
            }

            var defectList = detections.Where(x => x.DefectsCount > 0).GroupBy(x => x.DetectionTime.Date).Select(c => new
            {
                date = c.Key,
                count = c.Count()
            });
            foreach (var defect in defectList)
            {
                int index = generalInsightsDto.Labels.FindIndex(x => x.Equals(defect.date));
                if (index != -1)
                    generalInsightsDto.Defects[index] = defect.count;
            }

            return generalInsightsDto;
        }

        private async Task<GeneralInsightsDto> MonthlyInsightsAsync(int productId, int stageId, int type)
        {
            GeneralInsightsDto generalInsightsDto = new();
            List<AssemblyDetection.AssemblyDetection> detections = new();
            if (type == (byte)TypeEnum.FULL)
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-29));
            else if (type == (byte)TypeEnum.STAGE)
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-29)); 
            generalInsightsDto.TotalDetections = detections.Count;
            generalInsightsDto.TotalGood = detections.Where(x => x.DefectsCount == 0).Count();
            generalInsightsDto.TotalDefects = detections.Where(x => x.DefectsCount > 0).Count();
            var detectionList = detections.GroupBy(x => x.DetectionTime.Date).Select(c => new
            {
                date = c.Key,
                count = c.Count()
            });

            generalInsightsDto.Labels = detectionList.Select(x => x.date).ToList();
            generalInsightsDto.Detections = detectionList.Select(x => x.count).ToList();

            generalInsightsDto.Defects = new(new int[generalInsightsDto.Detections.Count]);
            generalInsightsDto.Good = new(new int[generalInsightsDto.Detections.Count]);

            var goodList = detections.Where(x => x.DefectsCount == 0).GroupBy(x => x.DetectionTime.Date).Select(c => new
            {
                date = c.Key,
                count = c.Count()
            });
            foreach (var good in goodList)
            {
                int index = generalInsightsDto.Labels.FindIndex(x => x.Equals(good.date));
                if (index != -1)
                    generalInsightsDto.Good[index] = good.count;
            }

            var defectList = detections.Where(x => x.DefectsCount > 0).GroupBy(x => x.DetectionTime.Date).Select(c => new
            {
                date = c.Key,
                count = c.Count()
            });
            foreach (var defect in defectList)
            {
                int index = generalInsightsDto.Labels.FindIndex(x => x.Equals(defect.date));
                if (index != -1)
                    generalInsightsDto.Defects[index] = defect.count;
            }

            return generalInsightsDto;
        }

        private async Task<GeneralInsightsDto> YearlyInsightsAsync(int productId, int stageId, int type)
        {
            GeneralInsightsDto generalInsightsDto = new();
            List<AssemblyDetection.AssemblyDetection> detections = new();
            if (type == (byte)TypeEnum.FULL)
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.DetectionTime.Date >= DateTime.Now.Date.AddMonths(-11));
            else if (type == (byte)TypeEnum.STAGE)
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId && x.DetectionTime.Date >= DateTime.Now.Date.AddMonths(-11));
            generalInsightsDto.TotalDetections = detections.Count;
            generalInsightsDto.TotalGood = detections.Where(x => x.DefectsCount == 0).Count();
            generalInsightsDto.TotalDefects = detections.Where(x => x.DefectsCount > 0).Count();
            var detectionList = detections.GroupBy(x => x.DetectionTime.Month);

            generalInsightsDto.Labels = detectionList.Select(x => x.ElementAt(0).DetectionTime.Date).ToList();
            generalInsightsDto.Detections = detectionList.Select(x => x.Count()).ToList();

            generalInsightsDto.Defects = new(new int[generalInsightsDto.Detections.Count]);
            generalInsightsDto.Good = new(new int[generalInsightsDto.Detections.Count]);

            var goodList = detections.Where(x => x.DefectsCount == 0).GroupBy(x => x.DetectionTime.Month);
            foreach (var good in goodList)
            {
                int index = generalInsightsDto.Labels.FindIndex(x => x.Equals(good.ElementAt(0).DetectionTime.Date));
                if (index != -1)
                    generalInsightsDto.Good[index] = good.Count();
            }

            var defectList = detections.Where(x => x.DefectsCount > 0).GroupBy(x => x.DetectionTime.Month);
            foreach (var defect in defectList)
            {
                int index = generalInsightsDto.Labels.FindIndex(x => x.Equals(defect.ElementAt(0).DetectionTime.Date));
                if (index != -1)
                    generalInsightsDto.Defects[index] = defect.Count();
            }

            return generalInsightsDto;
        }
    }
}