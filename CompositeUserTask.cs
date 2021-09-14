using Elsa.Activities.Console;
using Elsa.Activities.ControlFlow;
using Elsa.Attributes;
using Elsa.Builders;
using Elsa.Services;
using Elsa.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElsaQuickstarts.ConsoleApp.HelloWorld
{
    public class CompositeUserTask : CompositeActivity
    {
        [ActivityInput()]
        public ActorSelectionMode ActorSelectionMode { get => GetState<ActorSelectionMode>(); set => SetState(value); }

        [ActivityInput()]
        public IList<string> Actions { get => GetState(() => new List<string>()); set => SetState(value); }

        public override void Build(ICompositeActivityBuilder builder)
        {
            builder
            .StartWith<Fork>(x => x.WithBranches("UserTask", "TimeOut"), fork =>
            {
                fork
                    .When("UserTask")
                    .Then(() =>
                    {
                        Console.WriteLine("UserTask");
                    })
                    .Then<SingleUserTask>((setup) =>
                    {
                        setup.Set(x => x.Actions, () => Actions);
                        setup.Set(x => x.AssignUser, ResolveAssignAsync);
                        setup.Set(x => x.ActorSelectionMode, () => ActorSelectionMode);
                    })
                    .ThenNamed("Join");
            })
            .Add<Join>(join => join.Set(x => x.Mode, x => Join.JoinMode.WaitAny)).WithName("Join")
            .WriteLine("Container Joined!")
            .Finish(context => context.GetInput<string>());
        }

        private static async ValueTask<object> ResolveAssignAsync(ActivityExecutionContext context)
        {
            var parentActivityId = context.ActivityBlueprint.Parent?.Id;
            var mode = await context.WorkflowExecutionContext.GetActivityPropertyAsync<SingleUserTask, ActorSelectionMode>(parentActivityId, x => x.ActorSelectionMode)!;

            return "user1";
        }
    }
}
