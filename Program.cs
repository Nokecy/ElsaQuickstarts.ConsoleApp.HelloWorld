using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.Sqlite;
using Elsa.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ElsaQuickstarts.ConsoleApp.HelloWorld
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create a service container with Elsa services.
            var services = new ServiceCollection()
                .AddElsa(options => options
                    .AddConsoleActivities()
                    .AddActivity<SingleUserTask>()
                    .AddActivity<CompositeUserTask>()
                    .AddWorkflow<HelloWorld>()
                     .UseEntityFrameworkPersistence(ef => ef.UseSqlite(), autoRunMigrations: true)
                    )
                .BuildServiceProvider();

            // Get a workflow runner.
            var workflowRunner = services.GetRequiredService<IBuildsAndStartsWorkflow>();
            var workflowLaunchpad = services.GetRequiredService<IWorkflowLaunchpad>();

            // Run the workflow.
            var result = await workflowRunner.BuildAndStartWorkflowAsync<HelloWorld>();

            var blockingActiviti = result.WorkflowInstance.BlockingActivities.ToList().FirstOrDefault();

            await workflowLaunchpad.ExecutePendingWorkflowAsync(result.WorkflowInstance.Id, blockingActiviti.ActivityId);
        }
    }
}
