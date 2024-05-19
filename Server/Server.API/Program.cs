using Server.Application.Temp;
using Microsoft.EntityFrameworkCore;
using Server.Infrastructure.Persistence.Data;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Server.API.Mapping;
using Server.API.Hubs;

string settingsStream = "Server.API.appsettings.json";

var builder = WebApplication.CreateBuilder(args);

var a = Assembly.GetExecutingAssembly();
using var stream = a.GetManifestResourceStream(settingsStream);
builder.Configuration.AddJsonStream(stream);

builder.Services.AddSignalR();
// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});



var connStr = builder.Configuration.GetConnectionString("MySQLConnection");
ServerVersion vesrion = ServerVersion.AutoDetect(connStr);

var options = new DbContextOptionsBuilder<AppDbContext>()
                  .UseMySql(connStr, new MySqlServerVersion(new Version(8, 0, 36)))
                  .Options;

builder.Services.AddInfrastructure(options)
                .AddDbContext<AppDbContext>(opt => opt.UseMySql(connStr, new MySqlServerVersion(new Version(8, 0, 36))), ServiceLifetime.Scoped)
                .AddApplication();

//DbInitializer.Initialize(builder.Services.BuildServiceProvider()).Wait();

builder.Services.AddAutoMapper(typeof(AppMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<MessenderHub>("/messenger");

app.Run();

