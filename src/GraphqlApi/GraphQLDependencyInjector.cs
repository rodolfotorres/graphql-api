using System;
using System.Reflection;
using GraphQL.Conventions;

namespace GraphQLApi
{
    public class GraphQLDependencyInjector : IDependencyInjector
    {
        private readonly IServiceProvider _serviceProvider;

        public GraphQLDependencyInjector(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object Resolve(TypeInfo typeInfo) => _serviceProvider.GetService(typeInfo.AsType());
    }
}
