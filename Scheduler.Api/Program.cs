using Hangfire;
using Scheduler.Api.Endpoints;
using Scheduler.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(config =>
{
    config.UseSimpleAssemblyNameTypeSerializer();
    config.UseRecommendedSerializerSettings();
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"));
});

builder.Services.AddHangfireServer();

builder.Services.AddScoped<IJobSchedulerService, JobSchedulerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHangfireDashboard("/hangfire-dashboard", options: new DashboardOptions
{
    DashboardTitle = "Scheduler Background Jobs",
    DisplayStorageConnectionString = false,
    AppPath = null
});

app.MapJobShedulerEndpoints();
app.UseHttpsRedirection();

app.Run();
