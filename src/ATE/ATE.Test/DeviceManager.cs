using ATE.Test.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Test
{
    public class DeviceManager
    {
        public DeviceManager()
        {
            Devices = new();   
        }

        Dictionary<string, IDevice> Devices;


        public void AddDevice(IDevice device) { }

        public void SendCommand(string deviceName, string command)
        {
            if (Devices.ContainsKey(deviceName))
                Devices[deviceName].ExecuteCommand(command);
        }
    }
}
