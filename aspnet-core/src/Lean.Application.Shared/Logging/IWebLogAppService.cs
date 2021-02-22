using Abp.Application.Services;
using Lean.Dto;
using Lean.Logging.Dto;

namespace Lean.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
