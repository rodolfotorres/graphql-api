using System.Threading.Tasks;
using GraphQLApi.Interfaces;
using GraphQL.Conventions;
using GraphQL.Conventions.Relay;
using GraphQLApi.Schema.Types;

namespace GraphQLApi.Schema
{
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
          return Types.Team.FromDto(teamDto);
        }
    }
}
