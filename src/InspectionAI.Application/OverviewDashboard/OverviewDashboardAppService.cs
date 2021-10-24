using Abp.Domain.Repositories;
using InspectionAI.OverviewDashboard.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InspectionAI.OverviewDashboard
{
    public class OverviewDashboardAppService : IOverviewDashboardAppService
    {
        private readonly IRepository<Stage.Stage> _stageRepo;
        private readonly IRepository<AssemblyLine.AssemblyLine> _assemblyLineRepo;
        private readonly IRepository<Defects.Defects> _defectsRepo;
        private readonly IRepository<StageDefects.StageDefects> _stageDefectsRepo;
        private readonly IRepository<AssemblyDetection.AssemblyDetection> _assemblyDetectionRepo;
        private readonly IRepository<AssemblyDefects.AssemblyDefects> _assemblyDefectsRepo;
        private readonly IRepository<Product.Product> _productRepo;

        public OverviewDashboardAppService(IRepository<Stage.Stage> stageRepo,
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

        public async Task<OverviewGeneralInsightsDto> GetGeneralInsightsAsync(string duration)
        {
            if (duration.Equals("Weekly"))
            {
                return await WeeklyInsightsAsync();
            }
            else if (duration.Equals("Monthly"))
            {
                return await MonthlyInsightsAsync();
            }
            else if (duration.Equals("Yearly"))
            {
                return await YearlyInsightsAsync();
            }
            return new();
        }

        public async Task<OverviewRevenueLossDto> GetProductRevenueLoss(string duration)
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

        private async Task<OverviewRevenueLossDto> ProductRevenueLossWeeklyMonthly(string duration)
        {
            OverviewRevenueLossDto revenueLoss = new();
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

            revenueLoss.Data = new(new OverviewRevenueLossDataDto[products.Count]);
            for (int i = 0; i < products.Count; i++)
            {
                revenueLoss.Data[i] = new OverviewRevenueLossDataDto();
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

        private async Task<OverviewRevenueLossDto> ProductRevenueLossYearly(string duration)
        {
            OverviewRevenueLossDto revenueLoss = new();
            var products = await _productRepo.GetAllListAsync();
            var detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddMonths(-11));
            revenueLoss.Labels = detections.GroupBy(x => x.DetectionTime.Month).Select(x => x.ElementAt(0).DetectionTime.Date).ToList();
            revenueLoss.All = new(new double[revenueLoss.Labels.Count]);
            var defectives = detections.GroupBy(x => x.DetectionTime.Month);

            revenueLoss.Data = new(new OverviewRevenueLossDataDto[products.Count]);
            for (int i = 0; i < products.Count; i++)
            {
                revenueLoss.Data[i] = new OverviewRevenueLossDataDto();
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

        public async Task<OverviewDefectRatioDto> GetProductDefectRatio(string duration)
        {
            OverviewDefectRatioDto insightsDto = new();
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

        public async Task<OverviewDefectiveRatio> GetDefectiveProducts(string duration)
        {
            OverviewDefectiveRatio defectiveRatio = new();
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

        public async Task<OverviewDefectTrendDto> GetProductDefectTrend(string duration)
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

        private async Task<OverviewDefectTrendDto> ProductDefectTrendWeeklyMonthly(string duration)
        {
            OverviewDefectTrendDto defectTrend = new();
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
            defectTrend.Data = new(new OverviewDefectTrendDataDto[products.Count]);
            for (int i = 0; i < products.Count; i++)
            {
                defectTrend.Data[i] = new OverviewDefectTrendDataDto();
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

        private async Task<OverviewDefectTrendDto> ProductDefectTrendYearly(string duration)
        {
            OverviewDefectTrendDto defectTrend = new();
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
            defectTrend.Data = new(new OverviewDefectTrendDataDto[products.Count]);
            for (int i = 0; i < products.Count; i++)
            {
                defectTrend.Data[i] = new OverviewDefectTrendDataDto();
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
                    int index = defectTrend.Labels.FindIndex(y => y.Month == x.ElementAt(0).DetectionTime.Month);
                    if (index != -1)
                        defectData.Data[index] = x.Count();
                });
            }
            return defectTrend;
        }

        private async Task<OverviewGeneralInsightsDto> WeeklyInsightsAsync()
        {
            OverviewGeneralInsightsDto generalInsightsDto = new();
            List<AssemblyDetection.AssemblyDetection> detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-6));
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

        private async Task<OverviewGeneralInsightsDto> MonthlyInsightsAsync()
        {
            OverviewGeneralInsightsDto generalInsightsDto = new();
            List<AssemblyDetection.AssemblyDetection> detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-29));
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

        private async Task<OverviewGeneralInsightsDto> YearlyInsightsAsync()
        {
            OverviewGeneralInsightsDto generalInsightsDto = new();
            List<AssemblyDetection.AssemblyDetection> detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.DetectionTime.Date >= DateTime.Now.Date.AddMonths(-11));
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
                int index = generalInsightsDto.Labels.FindIndex(x => x.Month == good.ElementAt(0).DetectionTime.Month);
                if (index != -1)
                    generalInsightsDto.Good[index] = good.Count();
            }

            var defectList = detections.Where(x => x.DefectsCount > 0).GroupBy(x => x.DetectionTime.Month);
            foreach (var defect in defectList)
            {
                int index = generalInsightsDto.Labels.FindIndex(x => x.Month == defect.ElementAt(0).DetectionTime.Month);
                if (index != -1)
                    generalInsightsDto.Defects[index] = defect.Count();
            }

            return generalInsightsDto;
        }
    }
}