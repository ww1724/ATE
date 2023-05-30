using ATE.Test.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Test.TestingItem
{
    [TestingProject(name:"TerminateAteTestsss",title:"终止测试")]
    public class TerminateAteTest : ITestingProject
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string StepClass { get; set; }

        public void Run()
        {
            Console.WriteLine("Shut Down Current Test");
        }
    }
}
