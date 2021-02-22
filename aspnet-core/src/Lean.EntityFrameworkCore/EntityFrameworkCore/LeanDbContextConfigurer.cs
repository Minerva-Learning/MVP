using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Lean.EntityFrameworkCore
{
    public static class LeanDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<LeanDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<LeanDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}