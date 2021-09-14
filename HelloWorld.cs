using Elsa.Activities.Console;
using Elsa.Activities.ControlFlow;
using Elsa.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsaQuickstarts.ConsoleApp.HelloWorld
{
    public class HelloWorld : IWorkflow
    {
        public void Build(IWorkflowBuilder builder)
        {
            builder
                .WriteLine("Hello World!")
                 .Then<CompositeUserTask>((step) =>
                 {
                     step.Set(x => x.ActorSelectionMode, context => ActorSelectionMode.Single);
                     step.Set(x => x.Actions, context => new List<string>() { "Done", "Fail" });
                 }, (a) =>
                 {
                     a.When("Done").Then((a) =>
                     {
                         Console.WriteLine("Done");
                     }).ThenNamed("Exits");

                     a.When("Fail").Then((a) =>
                     {
                         Console.WriteLine("Fail");
                     }).ThenNamed("Exits");
                 })
                 .Add<Join>(step => step.Set(x => x.Mode, x => Join.JoinMode.WaitAny)).WithName("Exits")
                ;
        }
    }
}
