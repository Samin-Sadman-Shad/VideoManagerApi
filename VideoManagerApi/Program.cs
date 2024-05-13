using Microsoft.EntityFrameworkCore;
using VideoManagerApi.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Internal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProductVideoContext>(option =>
        option.UseMySql(connectionString: conn, serverVersion: ServerVersion.AutoDetect(conn)));
builder.Services.AddDistributedRedisCache(options =>
{
    options.InstanceName = "productVideoCache";
    options.Configuration = "localhost:6379";
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddLogging(builder =>
{
    builder.AddConsole();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
