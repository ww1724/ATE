using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Zoranof.Workflow.ATE
{
    [Node(title:"300P调光到最小",group:"ATE",name: "SetMinTo300P")]
    public class SetMinTo300P : TitleOnlyTemplate
    {
        public SetMinTo300P()
        {
            Title = "300P调光到最小";
        }
    }

    public class SetMinTo300PStep : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("300P调光到最小");
            return ExecutionResult.Next();
        }
    }
}
