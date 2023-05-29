using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Zoranof.Workflow.ATE
{
    [Node(title: "关闭电源", name:"ClosePower", group:"ATE")]
    public class ClosePower : TitleOnlyTemplate
    {
        public ClosePower()
        {
            Title = "关闭电源";
            StepBodyType = typeof(ClosePowerStep);
        }

      
    }

    public class ClosePowerStep : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Close Power");
            return ExecutionResult.Next();
        }
    }
}
