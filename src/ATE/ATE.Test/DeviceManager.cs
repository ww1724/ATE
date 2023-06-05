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

        public bool SendCommand(string deviceName, string command)
        {
            bool result = false;
            if (Devices.ContainsKey(deviceName))
               result =  Devices[deviceName].ExecuteCommand(command);
            return result;
        }

        public Task<bool> SendCommandAsync(string deviceName, string command, params object[] args)
        {
            return Task.FromResult(true);
        }
    }
}
