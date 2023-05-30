using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Test.Contract
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DeviceAtrribute : Attribute
    {
        public string DeviceName { get; set; }

        public string DeviceTitle { get; set; }

        public string DeviceType { get; set; }

        public string ControlProtocol { get; set; }

        public string DeviceDescritpion { get; set; }

        public DeviceAtrribute(string name, string title="",string devicetype="", string protocol="serialport", string description="")
        {
            DeviceName = name;
            DeviceTitle = title;
            DeviceType = devicetype;
            ControlProtocol = protocol;
            DeviceDescritpion = description;
        }
    }
}
