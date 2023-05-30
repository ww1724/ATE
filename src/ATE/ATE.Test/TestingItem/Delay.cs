using ATE.Test.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Test.TestingItem
{
    [TestingProject(name:"Delay", title:"延时")]
    public class Delay : ITestingProject
    {
        public string Title { get; set; }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
