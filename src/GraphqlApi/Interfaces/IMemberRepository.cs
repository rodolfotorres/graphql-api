using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQLApi.Models;

namespace GraphQLApi.Interfaces
{
    public interface IMemberRepository
    {
        Task<List<MemberDto>> GetMembersForTeam(string id);
        Task<MemberDto> AddMember(string teamId, MemberDto member);
    }
}
