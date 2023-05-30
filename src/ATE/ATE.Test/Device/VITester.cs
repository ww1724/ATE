using ATE.Test.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Test.Device
{
    internal class VITester : IDevice
    {
        public string DeviceName { get; set; }
        public bool IsConnected { get; set; }

        public VITester()
        {
                
        }

        public bool Connect(Dictionary<string, object> parameters)
        {
            return true;
        }

        public void InitializeDevice()
        {

        }

        public void ExecuteCommand(string command)
        {
            Console.WriteLine("Execute" + command);
        }

        public object GetValue(string key)
        {
            return null;
        }


    }
}
