using Abp.Dependency;
using GraphQL.Types;
using GraphQL.Utilities;
using Lean.Queries.Container;
using System;

namespace Lean.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IServiceProvider provider) :
            base(provider)
        {
            Query = provider.GetRequiredService<QueryContainer>();
        }
    }
}