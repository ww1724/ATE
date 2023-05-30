using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Test.Contract
{
    public interface ITestingProject
    {
        public string Title { get; set; }

        public abstract void Run();
    }
}
