using System.Threading.Tasks;
using GraphQLApi.Interfaces;
using GraphQLApi.Schema.Types;

namespace GraphQLApi.Repositories
{
    public class TeamRepository : ITeamRepository
    {
      public Task<TeamDto> GetTeamById(string id) => Task.FromResult(new TeamDto { Id = id, Name = $"Namefor: {id}"});
    }
}
