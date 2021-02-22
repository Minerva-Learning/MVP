using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Lean.Configuration;
using Lean.Web;

namespace Lean.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class LeanDbContextFactory : IDesignTimeDbContextFactory<LeanDbContext>
    {
        public LeanDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<LeanDbContext>();
            var configuration = AppConfigurations.Get(
                WebContentDirectoryFinder.CalculateContentRootFolder(),
                addUserSecrets: true
            );

            LeanDbContextConfigurer.Configure(builder, configuration.GetConnectionString(LeanConsts.ConnectionStringName));

            return new LeanDbContext(builder.Options);
        }
    }
}