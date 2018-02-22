using System.Threading.Tasks;
using GraphQLApi.Schema.Types;

namespace GraphQLApi.Interfaces
{
    public interface ITeamRepository
    {
        Task<TeamDto> GetTeamById(string id);
    }
}
