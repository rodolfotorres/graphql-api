using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQLApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GraphQL.Conventions.Web;
using GraphQL.Conventions;
using System.Reflection;
using static GraphQL.Conventions.Web.RequestHandler;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQLApi.Schema
{
  public class TeamSchema : ITeamSchema, IDependencyInjector
  {
    private readonly ResolveTypeDelegate _resolveTypeDelegate;
    private readonly IRequestHandler _requestHandler;
    public TeamSchema(ResolveTypeDelegate resolveTypeDelegate)
    {
        _resolveTypeDelegate = resolveTypeDelegate;
        _requestHandler = RequestHandler
          .New()
          .WithQuery<TeamsQuery>()
          .WithDependencyInjector(this)
          .WithoutValidation()
          .Generate();
    }

    public Task<Response> ProcessRequest(Request request) => _requestHandler.ProcessRequest(request, new UserContext());
    public object Resolve(TypeInfo typeInfo) => _resolveTypeDelegate?.Invoke(typeInfo.AsType());
  }
}
