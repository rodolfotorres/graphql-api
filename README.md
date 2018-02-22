.NET core mvc + GraphQL Conventions
====================================

## Introduction

This is a small tutorial on how to integrate [GraphQL Conventions](https://github.com/graphql-dotnet/conventions) with a .net core mvc app.

## Instructions

_Url_
http://localhost:5000/api/graphql/query

_Query_
```javascript
query _($teamId: String!) {
  viewer {
    team(id: $teamId) {
      id
      name
    }
  }
}
```
_Variables_
```javascript
{"teamId":"2252141"}
```

_Result_
```javascript
{
  "data": {
    "viewer": {
      "team": {
        "id": "VGVhbToyMjUyMTQx",
        "name": "Namefor: 2252141"
      }
    }
  }
}
```

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
        var query = await reader.ReadToEndAsync();
        var result = await _teamSchema.ProcessRequest(GraphQLWeb.Request.New(query));
        return result.HasErrors ? (IActionResult)BadRequest(result) : Ok(result.Body);
    }
}
```

_Schema_

```cs
public class TeamSchema : ITeamSchema
{
    private readonly IRequestHandler _requestHandler;
    public TeamSchema(IDependencyInjector dependencyInjector)
    {
        _requestHandler = RequestHandler
            .New()
            .WithQuery<TeamsQuery>()
            .WithDependencyInjector(dependencyInjector)
            .WithoutValidation()
            .Generate();
    }
    public Task<Response> ProcessRequest(Request request) => _requestHandler.ProcessRequest(request, new UserContext());
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
We provide a wrapper for .net core `IServiceProvider`
```cs
public void ConfigureServices(IServiceCollection services)
{
    services
        .AddSingleton<IDependencyInjector, GraphQLDependencyInjector>()
        .AddSingleton<ITeamRepository, TeamRepository>()
        .AddSingleton<TeamsQuery, TeamsQuery>()
        .AddSingleton<ITeamSchema, TeamSchema>()
        .AddMvc();
}

public class GraphQLDependencyInjector : IDependencyInjector
{
    private readonly IServiceProvider _serviceProvider;

    public GraphQLDependencyInjector(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public object Resolve(TypeInfo typeInfo) => _serviceProvider.GetService(typeInfo.AsType());
}
```
