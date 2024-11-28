using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Revisao.Application.Services.RabbitMQ;
using Revisao.Domain.Entities;
using Revisao.Domain.Interfaces;
using Revisao.Infra.Context;
using Revisao.IoC;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Registra o serviço IMessageQueue para enviar e consumir mensagens via RabbitMQ
builder.Services.AddSingleton<IMessageQueue>(sp =>
    new RabbitMqMessageQueue("amqp://dev:d3v12345@5.189.132.68:5673"));

// Registra o consumidor como Scoped ou Transient
builder.Services.AddScoped<OrderMessageConsumer>();

builder.Services.RegisterServices(builder.Configuration);



var app = builder.Build();


// Usa IServiceScopeFactory para resolver o OrderMessageConsumer
//using (var scope = app.Services.CreateScope())
//{
//    var consumer = scope.ServiceProvider.GetRequiredService<OrderMessageConsumer>();
//    consumer.StartConsuming();
//}

var cancellationTokenSource = new CancellationTokenSource();

// Iniciar o consumidor dentro de um escopo gerenciado
using (var scope = app.Services.CreateScope())
{
    var consumer = scope.ServiceProvider.GetRequiredService<OrderMessageConsumer>();
    _ = Task.Run(() => consumer.StartConsuming(cancellationTokenSource.Token));
}


// Middleware do Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API V1");
    });
}

app.UseAuthentication();
app.UseAuthorization();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
