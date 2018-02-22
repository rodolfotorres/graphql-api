using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQLApi.Interfaces;
using GraphQLWeb = GraphQL.Conventions.Web;
using Microsoft.AspNetCore.Mvc;

namespace GraphQLApi.Controllers
{
    [Route("api/[controller]")]
    public class GraphQLController : Controller
    {
        private readonly ITeamSchema _teamSchema;

        public GraphQLController(ITeamSchema teamSchema)
        {
            _teamSchema = teamSchema;
        }

        [HttpPost("query")]
        public async Task<IActionResult> Query()
        {
            StreamReader reader = new StreamReader(Request.Body);
            var query = await reader.ReadToEndAsync();
            var result = await _teamSchema.ProcessRequest(GraphQLWeb.Request.New(query));
            return result.HasErrors ? (IActionResult)BadRequest(result) : Ok(result.Body);
        }
    }
}
