using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNET_Core_COSMOS_DB.Models;
using ASPNET_Core_COSMOS_DB.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Rest.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace ASPNET_Core_COSMOS_DB
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
            services.AddSingleton(typeof(IDbCollectionOperationsRepository<PersonalInformationModel,string>), typeof(DbCollectionOperationsRepository));
            services.AddCors(op =>
                {
                    op.AddPolicy("AllowAll",
                        b=>b.WithOrigins("*").AllowAnyHeader().AllowAnyMethod()
                        );
                }
            );
            services.AddSwaggerGen(s=>
            {
                s.SwaggerDoc("v1",
                    new Info
                    {
                        Version = "v1",
                        Title = "Cosmos Db Integration",
                        Description = "Included technology-Azure cosmos db, angular4, swagger, oauth"
                    });
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseStaticFiles();
            app.UseCors("AllowAll");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
