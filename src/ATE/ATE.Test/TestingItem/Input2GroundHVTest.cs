using ATE.Test.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Test.TestingItem
{
    [TestingProject(name:"Input2GroundHVTest",title:"输入对地高压测试")]
    public class Input2GroundHVTest : ITestingProject
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string StepClass { get; set; }

        public void Run()
        {
            Console.WriteLine("Input2GroundHVTest");
        }
    }
}
