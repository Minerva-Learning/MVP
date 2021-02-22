using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Lean.MultiTenancy.Accounting.Dto;

namespace Lean.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
