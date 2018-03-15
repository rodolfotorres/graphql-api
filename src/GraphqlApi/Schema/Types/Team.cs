using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.Conventions;
using GraphQL.Conventions.Relay;
using GraphQLApi.Interfaces;
using GraphQLApi.Models;

namespace GraphQLApi.Schema.Types
{
    public class Team : INode
    {
        private TeamDto _dto;
        public static Team FromDto(TeamDto dto) => dto != null ? new Team { _dto = dto } : null;

        [Description("Id")]
        public Id Id => Id.New<Team>(_dto.Id);

        [Description("Name")]
        public string Name => _dto.Name;

        [Description("Member")]
        public async Task<List<Member>> Members([Inject] IMemberRepository memberRepository)
        {
            var members = await memberRepository.GetMembersForTeam(_dto.Id);
            return members.ConvertAll(Member.FromDto);
        }
    }
}
