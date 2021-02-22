using System.Collections.Generic;
using Lean.Authorization.Users.Importing.Dto;
using Lean.Dto;

namespace Lean.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
