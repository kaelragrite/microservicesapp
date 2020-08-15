using AutoMapper;
using Basket.API.Data;
using Basket.API.Data.Interfaces;
using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Producers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using StackExchange.Redis;

namespace Basket.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var config = ConfigurationOptions.Parse(Configuration.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(config);
            });

            services.AddTransient<IBasketContext, BasketContext>();
            services.AddTransient<IBasketRepository, BasketRepository>();

            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton<EventBusRabbitMqProducer>();
            services.AddSingleton<IRabbitMqConnection>(sp =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = Configuration["EventBus:HostName"],
                    UserName = Configuration["EventBus:Username"],
                    Password = Configuration["EventBus:Password"]
                };
                return new RabbitMqConnection(factory);
            });

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket API", Version = "v1" }));

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket API V1"));
        }
    }
}
