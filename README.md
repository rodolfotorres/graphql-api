.NET core mvc + GraphQL Conventions
====================================

## Introduction

This is a small tutorial on how to integrate [GraphQL Conventions](https://github.com/graphql-dotnet/conventions) with a .net core mvc app.

## Structure

On this example we want to expose Team information

_Controller_

```cs
[Route("api/[controller]")]
public class GraphQLController : Controller
{
  private readonly ITeamSchema _teamSchema;

  public GraphQLController(ITeamSchema teamSchema)
  {
      _teamSchema = teamSchema;
  }

  [HttpPost("query")]
  public async Task<IActionResult> Query()
  {
    StreamReader reader = new StreamReader(Request.Body);
    var query = reader.ReadToEnd();
    var result = await _teamSchema.ProcessRequest(GraphQLWeb.Request.New(query));
    return result.HasErrors ? (IActionResult)BadRequest(result) : Ok(result.Body);
  }
}
```

_Schema_

```cs
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
```

_Query_
```cs
[ImplementViewer(OperationType.Query)]
public class TeamsQuery
{
  private readonly ITeamRepository _teamRepository;

  public TeamsQuery(ITeamRepository teamRepository)
  {
      _teamRepository = teamRepository;
  }
  [Description("Team Query")]
  public async Task<Team> Team(NonNull<string> id)
  {
    var teamDto = await _teamRepository.GetTeamById(id);
    return new Team(teamDto);
  }
}
```

_Repository_
```cs
public class TeamRepository : ITeamRepository
{
  public Task<TeamDto> GetTeamById(string id) => Task.FromResult(new TeamDto { Id = id, Name = $"Namefor: {id}"});
}
```

_Type_

```cs
public class Team : INode
{
  private readonly TeamDto _dto;

  public Team(TeamDto dto)
  {
      _dto = dto;
  }
  [Description("Id")]
  public Id Id => Id.New<Team>(_dto.Id);

  [Description("Name")]
  public string Name => _dto.Name;
}
```

## Dependency Injection

Conventions needs a DI to resolve dependencies for graphql types. In this example `Team` type needs the `ITeamRepository` that is registered against .net core DI. 
We pass in a delegate as part of the Schema constructor so it can resolve it
```cs
public void ConfigureServices(IServiceCollection services)
{
    services
        .AddSingleton<ITeamRepository, TeamRepository>()
        .AddSingleton<TeamsQuery, TeamsQuery>()
        .AddSingleton<ITeamSchema>(c => new TeamSchema(ResolveReferenceType))
        .AddMvc();
}

public object ResolveReferenceType(Type type)
{
    return _serviceProvider.GetService(type);
}
```
