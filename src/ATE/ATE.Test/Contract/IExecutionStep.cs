using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Test.Contract
{
    public interface IExecutionStep
    {
        public void Execute(IExecutionContext context);
    }
}
