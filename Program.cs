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

// Define a CORS policy
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000") // React app's default port
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// Configure unified database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AppDatabase")));

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

app.UseStaticFiles(); // Add this line to serve static files from wwwroot

app.UseCors(MyAllowSpecificOrigins);

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
