using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Service.Entities
{
    public class TestingCodeEntity
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public int TargetProductId { get; set; }

        public string WorkflowCode { get; set; }

        public string FlowChartCode { get; set; }

        public string Description { get; set; }


    }
}
