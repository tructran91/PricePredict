﻿using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PricePrediction.API.Middlewares;
using PricePrediction.Application;
using PricePrediction.Application.Behaviors;
using PricePrediction.Application.Services;
using PricePrediction.Core.Repositories;
using PricePrediction.Infrastructure.Data;
using PricePrediction.Infrastructure.Repositories;
using PricePrediction.Infrastructure.Services;
using Refit;
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
                c.SwaggerDoc("v1", new() { Title = "Price Prediction API", Version = "v1" });
            });
        }

        public static void RegisterApplicationLayers(this WebApplicationBuilder builder)
        {
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddThirdPartyServices(typeof(AssemblyReference).Assembly, builder.Configuration);
        }

        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICandlestickService, CandlestickService>();
            //services.AddScoped<ITradingAnalysisService, TradingAnalysisService>();
        }

        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PricePredictContext>(c =>
                c.UseSqlServer(configuration.GetConnectionString("PricePredictConnection")));
            services.AddTransient<ExceptionHandlingMiddleware>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ITradeSignalRepository, TradeSignalRepository>();
            services.AddScoped<ITradeResultRepository, TradeResultRepository>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }

        public static void AddThirdPartyServices(this IServiceCollection services, Assembly assembly, IConfiguration configuration)
        {
            services.AddAutoMapper(assembly);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
            services.AddRefitClient<ICandlestickServiceRefit>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration["DataImportService:BaseUrl"]));
            services.AddValidatorsFromAssembly(assembly);
        }
    }
}
