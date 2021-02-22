using Abp.AspNetZeroCore;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;
using Lean.Configuration;
using Lean.EntityFrameworkCore;
using Lean.Migrator.DependencyInjection;
using System;

namespace Lean.Migrator
{
    [DependsOn(typeof(LeanEntityFrameworkCoreModule))]
    public class LeanMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public LeanMigratorModule(LeanEntityFrameworkCoreModule abpZeroTemplateEntityFrameworkCoreModule)
        {
            abpZeroTemplateEntityFrameworkCoreModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(LeanMigratorModule).GetAssembly().GetDirectoryPathOrNull(),
                environmentName: Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                addUserSecrets: true
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                LeanConsts.ConnectionStringName
                );
            Configuration.Modules.AspNetZero().LicenseCode = _appConfiguration["AbpZeroLicenseCode"];

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(typeof(IEventBus), () =>
            {
                IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                );
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LeanMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}