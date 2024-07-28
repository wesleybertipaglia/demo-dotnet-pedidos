using Pedidos.Application.Interfaces;
using Pedidos.Application.Services;
using Pedidos.Domain.Interfaces;
using Pedidos.Infrastructure.Extensions;
using Pedidos.Infrastructure.Repositories;
using Pedidos.API.Exceptions;
using AutoMapper;
using System.Reflection;

namespace Pedidos.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInMemoryDatabase();
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Pedidos.API",
                    Version = "v1",
                    Description = "API para gerenciar pedidos e produtos"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();

            services.AddScoped<IPedidoService, PedidoService>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();

            services.AddScoped<IItemPedidoService, ItemPedidoService>();
            services.AddScoped<IItemPedidoRepository, ItemPedidoRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pedidos.API v1");
                });
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
