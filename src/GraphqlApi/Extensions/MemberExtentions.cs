using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQLApi.Interfaces;
using GraphQLApi.Models;
using GraphQLApi.Schema.Types;

namespace GraphQLApi.Extensions
{
    public static class MemberExtentions
    {
        public static MemberDto ToDto(this MemberInput input) => new MemberDto { Name = input.Name };
    }
}
