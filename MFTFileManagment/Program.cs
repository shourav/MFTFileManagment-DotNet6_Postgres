global using Documents.Data;
global using Microsoft.EntityFrameworkCore;
global using Serilog;
using AutoMapper;
using MFTFileManagment;
using MFTFileManagment.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//add serilgger
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

//add auto mapper

var mapperConfig = new MapperConfiguration(mc => {
    mc.AddProfile(new FileProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

//builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add database service
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
