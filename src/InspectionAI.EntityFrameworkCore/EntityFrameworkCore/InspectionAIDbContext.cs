using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using InspectionAI.Authorization.Roles;
using InspectionAI.Authorization.Users;
using InspectionAI.MultiTenancy;

namespace InspectionAI.EntityFrameworkCore
{
    public class InspectionAIDbContext : AbpZeroDbContext<Tenant, Role, User, InspectionAIDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Product.Product> Products { get; set; }
        public DbSet<Stage.Stage> Stages { get; set; }
        public DbSet<Defects.Defects> Defects { get; set; }
        public DbSet<AssemblyLine.AssemblyLine> AssemblyLines { get; set; }
        public DbSet<AssemblyDetection.AssemblyDetection> AssemblyDetections { get; set; }
        public DbSet<StageDefects.StageDefects> StageDefects { get; set; }
        public DbSet<AssemblyDefects.AssemblyDefects> AssemblyDefects { get; set; }
        public InspectionAIDbContext(DbContextOptions<InspectionAIDbContext> options)
            : base(options)
        {
        }
    }
}
