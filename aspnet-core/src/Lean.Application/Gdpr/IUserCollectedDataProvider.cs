using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Lean.Dto;

namespace Lean.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
