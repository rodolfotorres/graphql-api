using System.Threading.Tasks;
using GraphQLApi.Interfaces;
using GraphQL.Conventions;
using GraphQL.Conventions.Relay;
using GraphQLApi.Schema.Types;
using GraphQLApi.Extensions;

namespace GraphQLApi.Schema
{
    [ImplementViewer(OperationType.Mutation)]
    public class TeamsMutation
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IMemberRepository _memberRepository;

        public TeamsMutation(ITeamRepository teamRepository, IMemberRepository memberRepository)
        {
            _teamRepository = teamRepository;
            _memberRepository = memberRepository;
        }

        [Description("Add a team member")]
        public async Task<Member> AddMember(NonNull<string> teamId, NonNull<MemberInput> memberInput)
        {
            var member = memberInput.Value.ToDto();
            var result = await _memberRepository.AddMember(teamId, member);
            var addMember = Member.FromDto(result);
            return addMember;
        }
    }
}
