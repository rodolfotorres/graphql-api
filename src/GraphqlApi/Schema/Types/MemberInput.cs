using System.Threading.Tasks;
using GraphQL.Conventions;
using GraphQL.Conventions.Relay;
using GraphQLApi.Interfaces;

namespace GraphQLApi.Schema.Types
{
    [InputType]
    public class MemberInput
    {
        [Description("Member name")]
        public NonNull<string> Name { get; set; }
    }
}
