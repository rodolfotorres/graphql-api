using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private ServiceProvider _serviceProvider;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<ITeamRepository, TeamRepository>()
                .AddSingleton<TeamsQuery, TeamsQuery>()
                .AddSingleton<ITeamSchema>(c => new TeamSchema(ResolveReferenceType))
                .AddMvc();
            _serviceProvider = services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        public object ResolveReferenceType(Type type)
        {
            return _serviceProvider.GetService(type);
        }
    }
}
