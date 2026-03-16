using Microsoft.EntityFrameworkCore;
using TeamTasks.Application.Abstractions.Repositories;
using TeamTasks.Application.Features.Dashboard.Queries.GetDeveloperDelayRisks;
using TeamTasks.Application.Features.Dashboard.Queries.GetDeveloperWorkloads;
using TeamTasks.Application.Features.Dashboard.Queries.GetProjectHealth;
using TeamTasks.Application.Features.Developers.Queries.GetActiveDevelopers;
using TeamTasks.Application.Features.Projects.Queries.GetProjectSummaries;
using TeamTasks.Application.Features.Projects.Queries.GetProjectTasks;
using TeamTasks.Application.Features.Tasks.Commands.CreateTask;
using TeamTasks.Application.Features.Tasks.Commands.UpdateTaskStatus;
using TeamTasks.Infrastructure.Persistence;
using TeamTasks.Infrastructure.Persistence.Repositories;
using TeamTasks.Infrastructure.Persistence.Seed;
using TeamTasks.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IDeveloperRepository, DeveloperRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();

builder.Services.AddScoped<GetProjectSummariesHandler>();
builder.Services.AddScoped<GetProjectTasksHandler>();
builder.Services.AddScoped<CreateTaskHandler>();
builder.Services.AddScoped<UpdateTaskStatusHandler>();
builder.Services.AddScoped<GetProjectHealthHandler>();
builder.Services.AddScoped<GetDeveloperWorkloadsHandler>();
builder.Services.AddScoped<GetDeveloperDelayRisksHandler>();
builder.Services.AddScoped<GetActiveDevelopersHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ApplicationDbContext>();

    await dbContext.Database.MigrateAsync();
    await ApplicationDbSeeder.SeedAsync(dbContext);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();