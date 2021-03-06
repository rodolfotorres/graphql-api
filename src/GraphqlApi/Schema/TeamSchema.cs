using System.Threading.Tasks;
using GraphQLApi.Interfaces;
using GraphQL.Conventions.Web;
using GraphQL.Conventions;

namespace GraphQLApi.Schema
{
    public class TeamSchema : ITeamSchema
    {
        private readonly IRequestHandler _requestHandler;
        public TeamSchema(IDependencyInjector dependencyInjector = null)
        {
            _requestHandler = RequestHandler
                .New()
                .WithQueryAndMutation<TeamsQuery, TeamsMutation>()
                //.WithSubscription<TeamsSubscription>()
                .WithDependencyInjector(dependencyInjector)
                .WithoutValidation()
                .Generate();
        }
        public Task<Response> ProcessRequest(Request request) => _requestHandler.ProcessRequest(request, new UserContext());

        public string Describe(bool returnJson = false) => _requestHandler.DescribeSchema(returnJson);
    }
}
