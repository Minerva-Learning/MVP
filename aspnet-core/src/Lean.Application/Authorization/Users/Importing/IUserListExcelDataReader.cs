using System.Collections.Generic;
using Lean.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace Lean.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
