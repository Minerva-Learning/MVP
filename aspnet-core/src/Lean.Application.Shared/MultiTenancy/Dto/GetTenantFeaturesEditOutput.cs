using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Lean.Editions.Dto;

namespace Lean.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}