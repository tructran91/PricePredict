using Microsoft.EntityFrameworkCore;
using PricePrediction.Application;
using PricePrediction.Core.Repositories;
using PricePrediction.Infrastructure.Data;
using PricePrediction.Infrastructure.Repositories;
using System.Reflection;

namespace PricePrediction.API.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureCorsAllowAny(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "Data Import API", Version = "v1" });
            });
        }

        public static void RegisterApplicationLayers(this WebApplicationBuilder builder)
        {
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddThirdPartyServices(typeof(AssemblyReference).Assembly);
        }

        public static void AddApplicationServices(this IServiceCollection services)
        {
            //services.AddScoped<IStorageService, LocalStorageService>();
            //services.AddScoped<IProductService, ProductService>();
        }

        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PricePredictContext>(c =>
                c.UseSqlServer(configuration.GetConnectionString("PricePredictConnection")));
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        }

        public static void AddThirdPartyServices(this IServiceCollection services, Assembly assembly)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        }
    }
}
