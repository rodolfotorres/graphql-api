using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraphQLApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using GraphQLWeb = GraphQL.Conventions.Web;

namespace GraphQLApi.Middleware
{
    public class GraphQLHttpMiddleware<TSchema> where TSchema : ISchema
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private readonly PathString _path;
        public GraphQLHttpMiddleware(ILogger<GraphQLHttpMiddleware<TSchema>> logger, RequestDelegate next, PathString path)
        {
            _logger = logger;
            _next = next;
            _path = path;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var httpRequest = context.Request;
            if (context.WebSockets.IsWebSocketRequest || !httpRequest.Path.StartsWithSegments(_path))
            {
                await _next(context);
                return;
            }
            var query = await ReadAsStringAsync(httpRequest.Body);
            var executer = context.RequestServices.GetRequiredService<TSchema>();
            var result = await executer.ProcessRequest(GraphQLWeb.Request.New(query));

            if (result.Errors?.Count > 0)
            {
                _logger.LogError("GraphQL execution error(s): {Errors}", result.Errors);
            }

            context.Response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            context.Response.StatusCode = result.Errors?.Count > 0 ? 400 : 200;
            await context.Response.WriteAsync(result.Body);
        }

        private static async Task<string> ReadAsStringAsync(Stream s)
        {
            using (var reader = new StreamReader(s))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
