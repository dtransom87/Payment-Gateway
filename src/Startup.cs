using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using Marten;
using Npgsql;
using payment_gateway.Extensions.Configuration;

namespace payment_gateway
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _environment = environment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "payment_gateway", Version = "v1" });
            });
            services.AddMvc().AddFluentValidation();

            services.ConfigureUtils();
            services.ConfigureValidators();
            services.ConfigureRepositories();
            services.ConfigureServices();

            services.AddMarten(opts =>
            {
                opts.Connection("Server=host.docker.internal;Port=5432;User ID=postgres;Password=test;Database=paymentgateway;Timeout=30;CommandTimeout=15;");
                if (_environment.IsDevelopment())
                {
                    opts.AutoCreateSchemaObjects = AutoCreate.All;
                    
                    opts.CreateDatabasesForTenants(c =>
                    {                   
                        c.MaintenanceDatabase("Server=host.docker.internal;Port=5432;User ID=postgres;Password=test;Database=paymentgateway;Timeout=30;CommandTimeout=15;");
                        c.ForTenant()
                            .CheckAgainstPgDatabase()
                            .WithOwner("postgres")
                            .WithEncoding("UTF-8")
                            .ConnectionLimit(-1);                         
                    });
            }});

            services.AddTransient<System.Data.IDbConnection, NpgsqlConnection>(sp =>
                new NpgsqlConnection("Server=host.docker.internal;Port=5432;User ID=postgres;Password=test;Database=paymentgateway;Timeout=30;CommandTimeout=15;"));    
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "payment_gateway v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
