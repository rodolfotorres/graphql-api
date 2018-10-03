using GraphQLApi.Interfaces;
using GraphQLApi.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseGraphQL<TSchema>(this IApplicationBuilder builder, string path = "/graphql")
    where TSchema : ISchema
    {
        return builder.UseGraphQL<TSchema>(new PathString(path));
    }

    public static IApplicationBuilder UseGraphQL<TSchema>(this IApplicationBuilder builder, PathString path)
    where TSchema : ISchema
    {
        return builder.UseMiddleware<GraphQLHttpMiddleware<TSchema>>(path);
    }
}
