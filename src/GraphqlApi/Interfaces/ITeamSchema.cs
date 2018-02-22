using System.Threading.Tasks;
using GraphQL.Conventions.Web;

namespace GraphQLApi.Interfaces
{
  public interface ITeamSchema
  {
      Task<Response> ProcessRequest(Request request);
  }
}
