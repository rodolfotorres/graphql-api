using System.Threading.Tasks;
using GraphQL.Conventions;
using GraphQL.Conventions.Relay;
using GraphQLApi.Interfaces;
using GraphQLApi.Models;

namespace GraphQLApi.Schema.Types
{
    public class Member : INode
    {
        private MemberDto _dto;
        public static Member FromDto(MemberDto dto) => dto != null ? new Member { _dto = dto } : null;

        [Description("Id")]
        public Id Id => Id.New<Member>(_dto.Id);

        [Description("Name")]
        public string Name => _dto.Name;
    }
}
