using System.Collections.Generic;
using Abp;
using Lean.Chat.Dto;
using Lean.Dto;

namespace Lean.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
