using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace InspectionAI.EntityFrameworkCore
{
    public static class InspectionAIDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<InspectionAIDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<InspectionAIDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
