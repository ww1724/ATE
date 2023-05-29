using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Zoranof.Workflow.ATE
{
    [Node(title: "300P调光到最大", group: "ATE", name: "SetMaxTo300P")]
    public class SetMaxTo300P : TitleOnlyTemplate
    {

        public SetMaxTo300P()
        {
            Title = "300P调光最大";
        }


    }

    public class SetMaxTo300PStep : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("300P调光到最大");
            return ExecutionResult.Next();
        }
    }
}
