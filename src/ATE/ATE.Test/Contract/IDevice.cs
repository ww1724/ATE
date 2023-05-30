using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Test.Contract
{
    public interface IDevice
    {
        public string DeviceName { get; set; }

        public bool IsConnected { get; set; }

        public abstract bool Connect(Dictionary<string, object> parameters);

        public abstract void InitializeDevice();

        public abstract void ExecuteCommand(string command);

        public abstract object GetValue(string key);
    }
}
