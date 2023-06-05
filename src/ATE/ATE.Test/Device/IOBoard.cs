using ATE.Test.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Test.Device
{
    [DeviceAtrribute(name:"AMotionETHIOBoard", title: "艾莫迅以太IO模块",devicetype:"Control",protocol:"ModbusTcp")]
    public class IOBoard : IDevice
    {
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
        public bool IsConnected { get; set; }
        public string DeviceDescription { get; set; }

        public bool Connect(Dictionary<string, object> parameters)
        {
            return true;
        }

        public void InitializeDevice()
        {
            
        }

        public bool ExecuteCommand(string command)
        {
            return true;
        }

        public Task<bool> ExecuteCommandAsync(string command, params object[] paras)
        {
            return Task.FromResult(true);
        }

        public object GetValue(string key)
        {
            return null;
        }




        // actions
    }
}
