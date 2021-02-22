using System.Threading.Tasks;
using Abp.Application.Services;
using Lean.Install.Dto;

namespace Lean.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}