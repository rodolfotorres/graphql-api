using System.Threading.Tasks;
using GraphQL.Conventions.Web;

namespace GraphQLApi.Interfaces
{
    public interface ISchema
  {
      Task<Response> ProcessRequest(Request request);
      string Describe(bool returnJson = false);
    }
}
