global using Documents.Data;
global using Microsoft.EntityFrameworkCore;
global using Serilog;
using MFTFileManagment;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//dependency injection
builder.Services.AddDbContext<FileDataContext>(options =>
     options.UseNpgsql(builder.Configuration.GetConnectionString("DocumentsDB"),
     b => b.MigrationsAssembly("MFTFileManagment"))
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Seed();
app.Run();
