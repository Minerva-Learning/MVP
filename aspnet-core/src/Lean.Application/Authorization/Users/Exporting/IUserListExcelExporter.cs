using System.Collections.Generic;
using Lean.Authorization.Users.Dto;
using Lean.Dto;

namespace Lean.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}