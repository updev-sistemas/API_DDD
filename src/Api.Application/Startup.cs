using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CrossCutting.DepedencyInjection;

namespace application
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
            ConfigureService.ConfigureDependenciesService(services);
            ConfigureRepository.ConfigureDependenciesRepository(services);
            services.AddControllers();
            services.AddSwaggerGen(x => {
                x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "1.0.0.0",
                    Title = "MyApi Versão 1",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Email = "falecomoantonio@live.com",
                        Name = "Antonio José",
                        Url = new System.Uri("http://suporte.updev.net.br")
                    },
                    Description = "API de Aprendizagem com arquitetura DDD",
                    License = new Microsoft.OpenApi.Models.OpenApiLicense
                    {
                        Url = new System.Uri("http://suporte.updev.net.br/licence"),
                        Name = "Termos de uso"
                    },
                    TermsOfService = new System.Uri("http://suporte.updev.net.br/licence")
                }); 
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json","MyApi");
                x.RoutePrefix = string.Empty;
            });

            app.UseRouting();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
