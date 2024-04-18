using Server.Application.Temp;
using Microsoft.EntityFrameworkCore;
using Server.Infrastructure.Persistence.Data;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
                     .GetConnectionString("MySQLConnection");
ServerVersion vesrion = ServerVersion.AutoDetect(connStr);

var options = new DbContextOptionsBuilder<AppDbContext>()
                  .UseMySql(connStr, new MySqlServerVersion(new Version(8, 0, 36)))
                  .Options;


var config = new ConfigurationManager();
builder.Services.AddInfrastructure(config, options)
                .AddApplication();

//DbInitializer.Initialize(builder.Services.BuildServiceProvider()).Wait();

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

