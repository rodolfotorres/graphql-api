using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using GraphQL.Conventions;
using GraphQL.Server.Ui.Playground;
using GraphQLApi.Interfaces;
using GraphQLApi.Repositories;
using GraphQLApi.Schema;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GraphQLApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<IDependencyInjector, GraphQLDependencyInjector>()
                .AddSingleton<IMemberRepository, MemberRepository>()
                .AddSingleton<ITeamRepository, TeamRepository>()
                .AddSingleton<TeamsQuery, TeamsQuery>()
                .AddSingleton<TeamsMutation, TeamsMutation>()
                .AddSingleton<TeamsSubscription, TeamsSubscription>()
                .AddSingleton<ITeamSchema, TeamSchema>()
                .AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions
            {
                Path = string.Empty,
                GraphQLEndPoint = "/api/graphql"
            });
            app.UseMvc();
        }
    }
}
