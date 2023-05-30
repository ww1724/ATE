using ATE.Test.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Test.TestingItem
{
    [TestingProject(name:"ExitAteTest",title:"退出测试")]
    public class ExitAteTest : ITestingProject
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string StepClass { get; set; }

        public void Run()
        {
            Console.WriteLine("Exit Ate Test Successfully");
        }
    }
}
