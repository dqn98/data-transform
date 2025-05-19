using DataTransform.Core.Interfaces;
using DataTransform.Core.Models;
using DataTransform.Data;
using DataTransform.Interfaces;
using DataTransform.Models;
using DataTransform.Repositories;
using DataTransform.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// Configure database contexts
builder.Services.AddDbContext<RawDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("RawDatabase")));

builder.Services.AddDbContext<ProcessedDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ProcessedDatabase")));

// Register repositories
builder.Services.AddScoped<IDataRepository<RawUserEvent>, RawDataRepository>();
builder.Services.AddScoped<IDataRepository<UserEvent>, ProcessedDataRepository>();

// Register services
builder.Services.AddScoped<IDataTransformService, DataTransformService>();
builder.Services.AddScoped<BackgroundJobService>();

// Register database seeder service
builder.Services.AddHostedService<DatabaseSeederService>();

// Configure Hangfire
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("HangfireDatabase")));

builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

// Configure Hangfire dashboard
app.UseHangfireDashboard();

app.MapControllers();

// Schedule recurring jobs
using (var scope = app.Services.CreateScope())
{
    var jobService = scope.ServiceProvider.GetRequiredService<BackgroundJobService>();
    jobService.ScheduleRecurringJobs();
}

app.Run();
