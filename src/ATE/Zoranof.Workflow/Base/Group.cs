using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoranof.WorkFlow;

namespace Zoranof.Workflow.Base
{
    public class Group : WorkflowNode
    {
        public Group()
        {

        }

        public List<WorkflowNode> Children { get; set; }

        public bool IsExpand { get; set; }

        public void SetExpandState(bool state)
        {
            IsExpand = state;
            AttachedView.InvalidateVisual();
        }
    }
}
