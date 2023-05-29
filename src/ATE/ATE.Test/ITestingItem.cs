using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Test
{
    public interface ITestingItem
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string StepClass { get; set; }

        public abstract void Run();
    }
}
