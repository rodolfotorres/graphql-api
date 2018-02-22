using GraphQL.Conventions;
using GraphQL.Conventions.Relay;

namespace GraphQLApi.Schema.Types
{
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
}
