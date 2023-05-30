using ATE.Test.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Test.TestingItem
{
    [TestingProject(name: "Output2GroundHVTest", title:"输出对地高压测试")]
    public class Output2GroundHVTest : ITestingProject
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string StepClass { get; set; }

        public void Run()
        {
            Console.WriteLine("Output2GroundHVTest");
        }
    }
}
