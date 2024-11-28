using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Revisao.Application.AutoMapper;
using Revisao.Application.Services._Order;
using Revisao.Application.Services._OrderItem;
using Revisao.Application.Services._JwtService;
using Revisao.Domain.Interfaces;
using Revisao.Infra.Context;
using Revisao.Infra.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revisao.Application.Services.RabbitMQ;

namespace Revisao.IoC;

public static class DependencyInjection
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration) 
    {
        services.AddDbContext<RevisaoContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddAutoMapper(typeof(DomainToDTOMapping));


        // Repository
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();


        // Services
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderItemService, OrderItemService>();
        services.AddScoped<JwtService>();

        //services.AddScoped<IMessageQueue, RabbitMqMessageQueue>();




        return services; 
    }
}
