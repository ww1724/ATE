using ATE.Test.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Test.TestingItem
{
    [TestingProject(name: "Input2OutputHVTest",title:"输入对输出高压测试")]
    public class Input2OutputHVTest : ITestingProject
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string StepClass { get; set; }

        public void Run()
        {
            Console.WriteLine("Input2OutputHVTest");
        }
    }
}
