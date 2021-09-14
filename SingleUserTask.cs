using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Builders;
using Elsa.Services;
using Elsa.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsaQuickstarts.ConsoleApp.HelloWorld
{
    public class SingleUserTask : Activity
    {
        [ActivityInput]
        public string AssignUser { get; set; }
        [ActivityInput]
        public IList<string> Actions { get; set; }
        [ActivityInput]
        public ActorSelectionMode ActorSelectionMode { get; set; }

        [ActivityOutput] public string Output { get; set; }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            return Suspend();
        }

        protected override IActivityExecutionResult OnResume(ActivityExecutionContext context)
        {
            var actorSelectionModel = context.WorkflowExecutionContext.GetActivityPropertyAsync<SingleUserTask, ActorSelectionMode>(context.ActivityId, x => x.ActorSelectionMode);

            var userAction = GetUserAction(context);
            Output = userAction;
            return Outcome(OutcomeNames.Done);
        }

        private static string GetUserAction(ActivityExecutionContext context) => (string)context.Input!;
    }
}
