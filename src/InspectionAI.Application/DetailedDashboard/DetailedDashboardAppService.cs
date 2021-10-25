using Abp.Domain.Repositories;
using InspectionAI.DetailedDashboard.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InspectionAI.DetailedDashboard
{
    public class DetailedDashboardAppService : IDetailedDashboardAppService
    {
        private readonly IRepository<Stage.Stage> _stageRepo;
        private readonly IRepository<AssemblyLine.AssemblyLine> _assemblyLineRepo;
        private readonly IRepository<Defects.Defects> _defectsRepo;
        private readonly IRepository<StageDefects.StageDefects> _stageDefectsRepo;
        private readonly IRepository<AssemblyDetection.AssemblyDetection> _assemblyDetectionRepo;
        private readonly IRepository<AssemblyDefects.AssemblyDefects> _assemblyDefectsRepo;

        public DetailedDashboardAppService(IRepository<Stage.Stage> stageRepo,
                              IRepository<AssemblyLine.AssemblyLine> assemblyLineRepo,
                              IRepository<Defects.Defects> defectsRepo,
                              IRepository<StageDefects.StageDefects> stageDefectsRepo,
                              IRepository<AssemblyDetection.AssemblyDetection> assemblyDetectionRepo,
                              IRepository<AssemblyDefects.AssemblyDefects> assemblyDefectsRepo)
        {
            _stageRepo = stageRepo;
            _assemblyLineRepo = assemblyLineRepo;
            _defectsRepo = defectsRepo;
            _stageDefectsRepo = stageDefectsRepo;
            _assemblyDetectionRepo = assemblyDetectionRepo;
            _assemblyDefectsRepo = assemblyDefectsRepo;
        }

        public async Task<DetailedGeneralInsightsDto> GetGeneralInsightsAsync(string duration,int productId, int stageId)
        {
            if (duration.Equals("Weekly"))
            {
                return await WeeklyInsightsAsync(productId, stageId);
            }
            else if (duration.Equals("Monthly"))
            {
                return await MonthlyInsightsAsync(productId, stageId);
            }
            else if (duration.Equals("Yearly"))
            {
                return await YearlyInsightsAsync(productId, stageId);
            }
            return new();
        }

        public async Task<DetailedDefectTrendDto> GetDefectTrendAsync(string duration, int productId, int stageId)
        {
            if (duration.Equals("Weekly"))
            {
                return await WeeklyDefectTrend(productId, stageId);
            }
            else if (duration.Equals("Monthly"))
            {
                return await MonthlyDefectTrend(productId, stageId);
            }
            else if (duration.Equals("Yearly"))
            {
                return await YearlyDefectTrend(productId, stageId);
            }
            return new();
        }

        public async Task<DetailedRevenueLossDto> GetRevenueLossAsync(string duration, int productId, int stageId)
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

        public async Task<Dto.DetailedAssemblyDefects> GetAssemblyDefectsAsync(string duration, int productId, int stageId)
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



        public async Task<DetailedDefectiveRatio> GetDefectiveRatio(string duration, int productId, int stageId)
        {
            DetailedDefectiveRatio defectiveRatio = new();
            List<AssemblyDefects.AssemblyDefects> defectsData = new List<AssemblyDefects.AssemblyDefects>();
            List<Defects.Defects> defects = await _defectsRepo.GetAllListAsync();

                if (duration.Equals("Weekly"))
                    defectsData = await _assemblyDefectsRepo.GetAllListAsync(x => x.StageId == stageId && x.DetectionTime >= DateTime.Now.Date.AddDays(-7));
                else if (duration.Equals("Monthly"))
                    defectsData = await _assemblyDefectsRepo.GetAllListAsync(x => x.StageId == stageId && x.DetectionTime >= DateTime.Now.Date.AddDays(-29));
                else if (duration.Equals("Yearly"))
                    defectsData = await _assemblyDefectsRepo.GetAllListAsync(x => x.StageId == stageId && x.DetectionTime >= DateTime.Now.Date.AddMonths(-11));


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

        private async Task<Dto.DetailedAssemblyDefects> WeeklyAssemblyDefects(int productId, int stageId)
        {
            Dto.DetailedAssemblyDefects assemblyDefects = new();
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

        private async Task<Dto.DetailedAssemblyDefects> MonthlyAssemblyDefects(int productId, int stageId)
        {
            Dto.DetailedAssemblyDefects assemblyDefects = new();
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

        private async Task<Dto.DetailedAssemblyDefects> YearlyAssemblyDefects(int productId, int stageId)
        {
            Dto.DetailedAssemblyDefects assemblyDefects = new();
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

        private async Task<DetailedRevenueLossDto> WeeklyRevenueLoss(int productId, int stageId)
        {
            DetailedRevenueLossDto revenueLoss = new();
            var detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId && x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-6));
            var detectionList = detections.OrderBy(x => x.DetectionTime.Date).GroupBy(x => x.DetectionTime.Date).ToList();

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
            revenueLoss.Data = new(new DetailedRevenueLossDataDto[assemblyLines.Count]);
            for (int i = 0; i < assemblyLines.Count; i++)
            {
                revenueLoss.Data[i] = new DetailedRevenueLossDataDto();
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

        private async Task<DetailedRevenueLossDto> MonthlyRevenueLoss(int productId, int stageId)
        {
            DetailedRevenueLossDto revenueLoss = new();
            var detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId && x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-29));
            var detectionList = detections.OrderBy(x => x.DetectionTime.Date).GroupBy(x => x.DetectionTime.Date).ToList();

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
            revenueLoss.Data = new(new DetailedRevenueLossDataDto[assemblyLines.Count]);
            for (int i = 0; i < assemblyLines.Count; i++)
            {
                revenueLoss.Data[i] = new DetailedRevenueLossDataDto();
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

        private async Task<DetailedRevenueLossDto> YearlyRevenueLoss(int productId, int stageId)
        {
            DetailedRevenueLossDto revenueLoss = new();
            var detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId && x.DefectsCount > 0 && x.DetectionTime >= DateTime.Now.Date.AddMonths(-11));
            var detectionList = detections.OrderBy(x => x.DetectionTime.Date).GroupBy(x => x.DetectionTime.Month).ToList();

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
            revenueLoss.Data = new(new DetailedRevenueLossDataDto[assemblyLines.Count]);
            for (int i = 0; i < assemblyLines.Count; i++)
            {
                revenueLoss.Data[i] = new DetailedRevenueLossDataDto();
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
                    int index = revenueLoss.Labels.FindIndex(y => y.Month == x.ElementAt(0).DetectionTime.Month);
                    if (index != -1)
                        revenueLossData.Data[index] = x.Count() * stage.Cost;
                });
            }
            return revenueLoss;
        }

        private async Task<DetailedDefectTrendDto> WeeklyDefectTrend(int productId, int stageId)
        {
            DetailedDefectTrendDto defectTrend = new();
            List<AssemblyDetection.AssemblyDetection> detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId && x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-6));
             detections = detections.OrderBy(x => x.DetectionTime.Date).ToList();
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


                var stageDefects = _stageDefectsRepo.GetAllIncluding(x => x.Defects).Where(x => x.ProductId == productId && x.StageId == stageId).ToList();
                defectTrend.Data = new(new DetailedDefectTrendDataDto[stageDefects.Count]);
                for (int i = 0; i < stageDefects.Count; i++)
                {
                    defectTrend.Data[i] = new DetailedDefectTrendDataDto();
                    defectTrend.Data[i].DefectId = stageDefects[i].DefectId;
                    defectTrend.Data[i].Name = stageDefects[i].Defects.Name;
                    defectTrend.Data[i].Data = new(new int[defectTrend.All.Count]);
                }
            
            List<AssemblyDefects.AssemblyDefects> detectionDefects = new();

                detectionDefects = await _assemblyDefectsRepo.GetAllListAsync(x => x.StageId == stageId && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-6));
            detectionDefects = detectionDefects.OrderBy(x => x.DetectionTime.Date).ToList();
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

        private async Task<DetailedDefectTrendDto> MonthlyDefectTrend(int productId, int stageId)
        {
            DetailedDefectTrendDto defectTrend = new();
            List<AssemblyDetection.AssemblyDetection> detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId && x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-29));
            detections = detections.OrderBy(x => x.DetectionTime.Date).ToList();
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

                var stageDefects = _stageDefectsRepo.GetAllIncluding(x => x.Defects).Where(x => x.ProductId == productId && x.StageId == stageId).ToList();
                defectTrend.Data = new(new DetailedDefectTrendDataDto[stageDefects.Count]);
                for (int i = 0; i < stageDefects.Count; i++)
                {
                    defectTrend.Data[i] = new DetailedDefectTrendDataDto();
                    defectTrend.Data[i].DefectId = stageDefects[i].DefectId;
                    defectTrend.Data[i].Name = stageDefects[i].Defects.Name;
                    defectTrend.Data[i].Data = new(new int[defectTrend.All.Count]);
                }

            List<AssemblyDefects.AssemblyDefects> detectionDefects = new();

                detectionDefects = await _assemblyDefectsRepo.GetAllListAsync(x => x.StageId == stageId && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-29));
            detectionDefects = detectionDefects.OrderBy(x => x.DetectionTime.Date).ToList();
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

        private async Task<DetailedDefectTrendDto> YearlyDefectTrend(int productId, int stageId)
        {
            DetailedDefectTrendDto defectTrend = new();
            List<AssemblyDetection.AssemblyDetection> detections = new();
                detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId && x.DefectsCount > 0 && x.DetectionTime.Date >= DateTime.Now.Date.AddMonths(-11));
            detections = detections.OrderBy(x => x.DetectionTime.Date).ToList();
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

                var stageDefects = _stageDefectsRepo.GetAllIncluding(x => x.Defects).Where(x => x.ProductId == productId && x.StageId == stageId).ToList();
                defectTrend.Data = new(new DetailedDefectTrendDataDto[stageDefects.Count]);
                for (int i = 0; i < stageDefects.Count; i++)
                {
                    defectTrend.Data[i] = new DetailedDefectTrendDataDto();
                    defectTrend.Data[i].DefectId = stageDefects[i].DefectId;
                    defectTrend.Data[i].Name = stageDefects[i].Defects.Name;
                    defectTrend.Data[i].Data = new(new int[defectTrend.All.Count]);
                }

            List<AssemblyDefects.AssemblyDefects> detectionDefects = new();

                detectionDefects = await _assemblyDefectsRepo.GetAllListAsync(x => x.StageId == stageId && x.DetectionTime.Date >= DateTime.Now.Date.AddMonths(-11));
             detectionDefects = detectionDefects.OrderBy(x => x.DetectionTime.Date).ToList();
            foreach (var defectData in defectTrend.Data)
            {
                var data = detectionDefects.Where(x => x.DefectId == defectData.DefectId)
                    .GroupBy(x => x.DetectionTime.Month).ToList();

                foreach(var defectGroup in data)
                {
                    int index = defectTrend.Labels.FindIndex(y => y.Month == defectGroup.ElementAt(0).DetectionTime.Month);
                    if (index != -1)
                        defectData.Data[index] = defectGroup.Count();
                }
            }
            return defectTrend;
        }

        private async Task<DetailedGeneralInsightsDto> WeeklyInsightsAsync(int productId, int stageId)
        {
            DetailedGeneralInsightsDto generalInsightsDto = new();

            List<AssemblyDetection.AssemblyDetection> detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-6));
            detections = detections.OrderBy(x => x.DetectionTime.Date).ToList();
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

        private async Task<DetailedGeneralInsightsDto> MonthlyInsightsAsync(int productId, int stageId)
        {
            DetailedGeneralInsightsDto generalInsightsDto = new();
            List<AssemblyDetection.AssemblyDetection> detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId && x.DetectionTime.Date >= DateTime.Now.Date.AddDays(-29));
            detections = detections.OrderBy(x => x.DetectionTime.Date).ToList();
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

        private async Task<DetailedGeneralInsightsDto> YearlyInsightsAsync(int productId, int stageId)
        {
            DetailedGeneralInsightsDto generalInsightsDto = new();

            List<AssemblyDetection.AssemblyDetection> detections = await _assemblyDetectionRepo.GetAllListAsync(x => x.ProductId == productId && x.StageId == stageId && x.DetectionTime.Date >= DateTime.Now.Date.AddMonths(-11));
            detections = detections.OrderBy(x => x.DetectionTime.Date).ToList();
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