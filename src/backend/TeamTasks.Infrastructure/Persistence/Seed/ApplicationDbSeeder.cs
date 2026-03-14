using Microsoft.EntityFrameworkCore;
using TeamTasks.Domain.Entities;
using TeamTasks.Domain.Enums;

namespace TeamTasks.Infrastructure.Persistence.Seed;

public static class ApplicationDbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
    {
        var hasData =
            await context.Developers.AnyAsync(cancellationToken) ||
            await context.Projects.AnyAsync(cancellationToken) ||
            await context.Tasks.AnyAsync(cancellationToken);

        if (hasData)
        {
            return;
        }

        var utcNow = DateTime.UtcNow;
        var today = utcNow.Date;

        var developers = new List<Developer>
        {
            new()
            {
                FirstName = "Ana",
                LastName = "Martinez",
                Email = "ana.martinez@teamtasks.dev",
                IsActive = true,
                CreatedAt = utcNow.AddDays(-60)
            },
            new()
            {
                FirstName = "Carlos",
                LastName = "Gomez",
                Email = "carlos.gomez@teamtasks.dev",
                IsActive = true,
                CreatedAt = utcNow.AddDays(-55)
            },
            new()
            {
                FirstName = "Laura",
                LastName = "Ramirez",
                Email = "laura.ramirez@teamtasks.dev",
                IsActive = true,
                CreatedAt = utcNow.AddDays(-50)
            },
            new()
            {
                FirstName = "Miguel",
                LastName = "Torres",
                Email = "miguel.torres@teamtasks.dev",
                IsActive = true,
                CreatedAt = utcNow.AddDays(-45)
            },
            new()
            {
                FirstName = "Sofia",
                LastName = "Castro",
                Email = "sofia.castro@teamtasks.dev",
                IsActive = true,
                CreatedAt = utcNow.AddDays(-40)
            }
        };

        await context.Developers.AddRangeAsync(developers, cancellationToken);

        var projects = new List<Project>
        {
            new()
            {
                Name = "Customer Portal Revamp",
                ClientName = "Northwind Traders",
                StartDate = today.AddDays(-30),
                EndDate = today.AddDays(30),
                Status = ProjectStatus.InProgress,
                CreatedAt = utcNow.AddDays(-30)
            },
            new()
            {
                Name = "Mobile Sales App",
                ClientName = "Contoso Retail",
                StartDate = today.AddDays(-10),
                EndDate = today.AddDays(45),
                Status = ProjectStatus.InProgress,
                CreatedAt = utcNow.AddDays(-10)
            },
            new()
            {
                Name = "Legacy Billing Migration",
                ClientName = "Fabrikam Energy",
                StartDate = today.AddDays(-90),
                EndDate = today.AddDays(-5),
                Status = ProjectStatus.Completed,
                CreatedAt = utcNow.AddDays(-90)
            }
        };

        await context.Projects.AddRangeAsync(projects, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        var tasks = new List<TaskItem>
        {
            new()
            {
                ProjectId = projects[0].ProjectId,
                Title = "Define project backlog",
                Description = "Create initial backlog and priorities.",
                AssigneeId = developers[0].DeveloperId,
                Status = TaskItemStatus.Completed,
                Priority = TaskPriority.High,
                EstimatedComplexity = 3,
                DueDate = today.AddDays(-20),
                CompletionDate = today.AddDays(-18),
                CreatedAt = utcNow.AddDays(-28)
            },
            new()
            {
                ProjectId = projects[0].ProjectId,
                Title = "Implement authentication module",
                Description = "Add JWT authentication and authorization.",
                AssigneeId = developers[1].DeveloperId,
                Status = TaskItemStatus.InProgress,
                Priority = TaskPriority.High,
                EstimatedComplexity = 5,
                DueDate = today.AddDays(3),
                CompletionDate = null,
                CreatedAt = utcNow.AddDays(-14)
            },
            new()
            {
                ProjectId = projects[0].ProjectId,
                Title = "Build dashboard widgets",
                Description = "Create summary cards and charts.",
                AssigneeId = developers[2].DeveloperId,
                Status = TaskItemStatus.ToDo,
                Priority = TaskPriority.Medium,
                EstimatedComplexity = 4,
                DueDate = today.AddDays(6),
                CompletionDate = null,
                CreatedAt = utcNow.AddDays(-7)
            },
            new()
            {
                ProjectId = projects[0].ProjectId,
                Title = "Fix pagination bug",
                Description = "Resolve issue in task listing pagination.",
                AssigneeId = developers[3].DeveloperId,
                Status = TaskItemStatus.Blocked,
                Priority = TaskPriority.Medium,
                EstimatedComplexity = 2,
                DueDate = today.AddDays(-1),
                CompletionDate = null,
                CreatedAt = utcNow.AddDays(-5)
            },
            new()
            {
                ProjectId = projects[0].ProjectId,
                Title = "Optimize SQL queries",
                Description = "Improve project summary query performance.",
                AssigneeId = developers[4].DeveloperId,
                Status = TaskItemStatus.InProgress,
                Priority = TaskPriority.High,
                EstimatedComplexity = 4,
                DueDate = today.AddDays(2),
                CompletionDate = null,
                CreatedAt = utcNow.AddDays(-6)
            },
            new()
            {
                ProjectId = projects[0].ProjectId,
                Title = "Prepare UAT package",
                Description = "Bundle release candidate for user acceptance testing.",
                AssigneeId = developers[0].DeveloperId,
                Status = TaskItemStatus.ToDo,
                Priority = TaskPriority.Low,
                EstimatedComplexity = 2,
                DueDate = today.AddDays(10),
                CompletionDate = null,
                CreatedAt = utcNow.AddDays(-2)
            },

            new()
            {
                ProjectId = projects[1].ProjectId,
                Title = "Create mobile shell",
                Description = "Set up base mobile app structure.",
                AssigneeId = developers[1].DeveloperId,
                Status = TaskItemStatus.Completed,
                Priority = TaskPriority.Medium,
                EstimatedComplexity = 3,
                DueDate = today.AddDays(-8),
                CompletionDate = today.AddDays(-7),
                CreatedAt = utcNow.AddDays(-18)
            },
            new()
            {
                ProjectId = projects[1].ProjectId,
                Title = "Implement offline sync",
                Description = "Create local sync strategy for intermittent connectivity.",
                AssigneeId = developers[2].DeveloperId,
                Status = TaskItemStatus.InProgress,
                Priority = TaskPriority.High,
                EstimatedComplexity = 5,
                DueDate = today.AddDays(5),
                CompletionDate = null,
                CreatedAt = utcNow.AddDays(-9)
            },
            new()
            {
                ProjectId = projects[1].ProjectId,
                Title = "Build product catalog screen",
                Description = "Display products with search and filters.",
                AssigneeId = developers[3].DeveloperId,
                Status = TaskItemStatus.ToDo,
                Priority = TaskPriority.Medium,
                EstimatedComplexity = 3,
                DueDate = today.AddDays(7),
                CompletionDate = null,
                CreatedAt = utcNow.AddDays(-8)
            },
            new()
            {
                ProjectId = projects[1].ProjectId,
                Title = "Integrate push notifications",
                Description = "Configure notification provider integration.",
                AssigneeId = developers[4].DeveloperId,
                Status = TaskItemStatus.Blocked,
                Priority = TaskPriority.High,
                EstimatedComplexity = 4,
                DueDate = today.AddDays(-2),
                CompletionDate = null,
                CreatedAt = utcNow.AddDays(-6)
            },
            new()
            {
                ProjectId = projects[1].ProjectId,
                Title = "Implement order history",
                Description = "Create order history endpoint consumption.",
                AssigneeId = developers[0].DeveloperId,
                Status = TaskItemStatus.InProgress,
                Priority = TaskPriority.Medium,
                EstimatedComplexity = 3,
                DueDate = today.AddDays(1),
                CompletionDate = null,
                CreatedAt = utcNow.AddDays(-4)
            },
            new()
            {
                ProjectId = projects[1].ProjectId,
                Title = "Refine app navigation",
                Description = "Improve menu and screen transitions.",
                AssigneeId = developers[1].DeveloperId,
                Status = TaskItemStatus.ToDo,
                Priority = TaskPriority.Low,
                EstimatedComplexity = 2,
                DueDate = today.AddDays(12),
                CompletionDate = null,
                CreatedAt = utcNow.AddDays(-3)
            },
            new()
            {
                ProjectId = projects[1].ProjectId,
                Title = "Add telemetry events",
                Description = "Track critical app usage events.",
                AssigneeId = developers[2].DeveloperId,
                Status = TaskItemStatus.Completed,
                Priority = TaskPriority.Low,
                EstimatedComplexity = 2,
                DueDate = today.AddDays(-6),
                CompletionDate = today.AddDays(-2),
                CreatedAt = utcNow.AddDays(-12)
            },

            new()
            {
                ProjectId = projects[2].ProjectId,
                Title = "Analyze legacy schema",
                Description = "Review current billing schema and dependencies.",
                AssigneeId = developers[3].DeveloperId,
                Status = TaskItemStatus.Completed,
                Priority = TaskPriority.High,
                EstimatedComplexity = 4,
                DueDate = today.AddDays(-60),
                CompletionDate = today.AddDays(-55),
                CreatedAt = utcNow.AddDays(-75)
            },
            new()
            {
                ProjectId = projects[2].ProjectId,
                Title = "Migrate invoice history",
                Description = "Transfer invoice data to the new structure.",
                AssigneeId = developers[4].DeveloperId,
                Status = TaskItemStatus.Completed,
                Priority = TaskPriority.High,
                EstimatedComplexity = 5,
                DueDate = today.AddDays(-45),
                CompletionDate = today.AddDays(-40),
                CreatedAt = utcNow.AddDays(-70)
            },
            new()
            {
                ProjectId = projects[2].ProjectId,
                Title = "Reconcile payment records",
                Description = "Validate migrated payment records.",
                AssigneeId = developers[0].DeveloperId,
                Status = TaskItemStatus.Completed,
                Priority = TaskPriority.Medium,
                EstimatedComplexity = 4,
                DueDate = today.AddDays(-35),
                CompletionDate = today.AddDays(-30),
                CreatedAt = utcNow.AddDays(-60)
            },
            new()
            {
                ProjectId = projects[2].ProjectId,
                Title = "Generate cutover checklist",
                Description = "Prepare production cutover checklist.",
                AssigneeId = developers[1].DeveloperId,
                Status = TaskItemStatus.Completed,
                Priority = TaskPriority.Medium,
                EstimatedComplexity = 2,
                DueDate = today.AddDays(-25),
                CompletionDate = today.AddDays(-22),
                CreatedAt = utcNow.AddDays(-50)
            },
            new()
            {
                ProjectId = projects[2].ProjectId,
                Title = "Validate tax calculation rules",
                Description = "Compare old and new tax calculations.",
                AssigneeId = developers[2].DeveloperId,
                Status = TaskItemStatus.Completed,
                Priority = TaskPriority.High,
                EstimatedComplexity = 5,
                DueDate = today.AddDays(-20),
                CompletionDate = today.AddDays(-14),
                CreatedAt = utcNow.AddDays(-40)
            },
            new()
            {
                ProjectId = projects[2].ProjectId,
                Title = "Prepare production support handoff",
                Description = "Document support procedures and contacts.",
                AssigneeId = developers[3].DeveloperId,
                Status = TaskItemStatus.Completed,
                Priority = TaskPriority.Low,
                EstimatedComplexity = 2,
                DueDate = today.AddDays(-12),
                CompletionDate = today.AddDays(-10),
                CreatedAt = utcNow.AddDays(-25)
            },
            new()
            {
                ProjectId = projects[2].ProjectId,
                Title = "Close migration retrospective",
                Description = "Capture lessons learned and final report.",
                AssigneeId = developers[4].DeveloperId,
                Status = TaskItemStatus.Completed,
                Priority = TaskPriority.Low,
                EstimatedComplexity = 1,
                DueDate = today.AddDays(-7),
                CompletionDate = today.AddDays(-7),
                CreatedAt = utcNow.AddDays(-15)
            }
        };

        await context.Tasks.AddRangeAsync(tasks, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}