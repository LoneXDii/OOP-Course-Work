using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Infrastructure.Persistence.Data;
using System.Reflection;

string settingsStream = "Server.API.appsettings.json";

var builder = WebApplication.CreateBuilder(args);

var a = Assembly.GetExecutingAssembly();
using var stream = a.GetManifestResourceStream(settingsStream);
builder.Configuration.AddJsonStream(stream);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connStr = builder.Configuration
                     .GetConnectionString("MSSQLServerConnection");

var options = new DbContextOptionsBuilder<AppDbContext>()
                  .UseSqlServer(connStr)
                  .Options;


var config = new ConfigurationManager();
builder.Services.AddInfrastructure(config, options)
                .AddApplication();

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

