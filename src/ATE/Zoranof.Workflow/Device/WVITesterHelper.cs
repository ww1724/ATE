using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoranof.Workflow.Device
{
    public enum WVISystemSettings
    {
        G_NPLC,
        R_NPLC,
        G_PLC,
        R_PLC
    }

    public enum WVITestingMode
    {
        None,
        IR,
        ACW,
        ACW_IR,
        IR_ACW
    }

    public enum WVITestingStatus
    {
        None,
        Testing,
        Pass,
        Stop,
        Error
    }

    public enum WVITestingErrorType
    {
        None,
        Timeout,
        ErrorParameter,
        TestingFail
    }

    public class WVITestingResult
    {
        public WVITestingStatus ResultType { get; set; }
        public WVITestingErrorType ErrorType { get; set; }

        public byte[] Data { get; set; }
        public byte ResultData { get; set; }

        public int WVoltage { get; set; }
        public double WCurrent { get; set; }
        public double WRemainingTime { get; set; }
        public int IVoltage {get;set;}
        public double ResistanceValue { get; set; }
        public double IRemainingTime { get;set; }

        public byte TestingStatus { get; set; } 
        public bool IRUpperResult { get; set; }
        public bool IRLowerResult { get; set; }
        public bool IRResult { get; set; }
        public bool ACWUpperResult { get; set; }
        public bool ACWLowwerResult { get; set; }
        public bool ACWResult { get; set; }
    }

    public class WVITesterHelper
    {
        public static byte[] OkBuffer = { 0x7B, 0x06, 0x4F, 0x4B, 0xA0, 0x7D };

        public bool DeviceSelfTest(string deviceport, string addr)
        {
            SerialPort serialport = new SerialPort(deviceport, 9600, Parity.None, 8, StopBits.One);


            return true;
        }

        /// <summary>
        /// 启动测试
        /// </summary>
        /// <param name="voltage"></param>
        /// <param name="deviceport"></param>
        /// <param name="addr"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool StartTest(string deviceport, string addr, int timeout=1000)
        {
            byte[] command = { 0x7B, 0x06, Convert.ToByte(addr), 0x02, 0x08, 0x7D };
            
            SerialPort serialPort = new SerialPort(deviceport, 9600, Parity.None, 8, StopBits.One);
            serialPort.Open();
            serialPort.Write(command, 0, command.Length);
            Thread.Sleep(100);
            int index = 0;
            while(index > timeout / 10 || serialPort.BytesToRead > 0)
            {
                Thread.Sleep(10);
                index++;
            }
            if (index > timeout / 10) return false;
            byte[] buffer = { };
            serialPort.Read(buffer, 0, 6);
            serialPort.DiscardInBuffer();
            if (buffer.SequenceEqual(OkBuffer)) return true;
            else return false;
        }

        /// <summary>
        /// 停止测试（也可用于测试仪器通信是否正常）
        /// </summary>
        /// <param name="deviceport"></param>
        /// <param name="addr"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool StopTest(string deviceport, string addr, int timeout=1000)
        {
            byte[] command = { 0x7B, 0x06, Convert.ToByte(addr), 0x02, 0x08, 0x7D };
            
            SerialPort serialPort = new SerialPort(deviceport, 9600, Parity.None, 8, StopBits.One);
            serialPort.Open();
            serialPort.Write(command, 0, command.Length);
            int index = 0;
            while(index > timeout / 50 || serialPort.BytesToRead > 0)
            {
                Thread.Sleep(50);
                index++;
            }
            if (index > timeout / 50) return false;
            byte[] buffer = { };
            serialPort.Read(buffer, 0, 6);
            serialPort.DiscardInBuffer();
            if (buffer.SequenceEqual(OkBuffer)) return true;
            else return false;
        }
   
        /// <summary>
        /// 读取设备测试结果
        /// </summary>
        /// <param name="deviceport"></param>
        /// <param name="addr"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public WVITestingResult ReadTestingResult(string deviceport, string addr, int timeout = 1000)
        {
            byte[] command = { 0x7B, 0x06, Convert.ToByte(addr), 0x00, 0x06, 0x7D };

            SerialPort serialPort = new SerialPort(deviceport, 9600, Parity.None, 8, StopBits.One);
            serialPort.Open();
            serialPort.Write(command, 0, command.Length);
            int index = 0;
            while (index > timeout / 10 || serialPort.BytesToRead > 0)
            {
                Thread.Sleep(10);
                index++;
            }
            if (index > timeout / 10) return new WVITestingResult() { ResultType= WVITestingStatus.Error, ErrorType=WVITestingErrorType.Timeout };
            byte[] buffer = { };
            serialPort.Read(buffer, 0, 6);
            serialPort.DiscardInBuffer();
            if (buffer.SequenceEqual(OkBuffer)) return new WVITestingResult() { ResultType=WVITestingStatus.Pass, Data=buffer };
            else return new WVITestingResult() { ResultType=WVITestingStatus.Error, ErrorType=WVITestingErrorType.ErrorParameter };
        }
    
        /// <summary>
        /// 解析测试结果
        /// </summary>
        /// <param name="result"></param>
        /// <param name="info"></param>
        public void ParseTestingResult(ref WVITestingResult result, ref string info) { 
            
        }
    
        /// <summary>
        /// 设置测试模式
        /// </summary>
        /// <param name="deviceport"></param>
        /// <param name="addr"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool SetTestingMode(WVITestingMode mode, string deviceport, string addr, int timeout = 1000)
        {
            int _retLength = 6;

            byte[] command = { 0x7B, 0x07, Convert.ToByte(addr), 0x03, (byte)mode, 0x0A, 0x7D };
            SerialPort serialPort = new SerialPort(deviceport, 9600, Parity.None, 8, StopBits.One);
            serialPort.Open();
            serialPort.Write(command, 0, command.Length);
            int index = 0;
            while (index > timeout / 10 || serialPort.BytesToRead >= _retLength)
            {
                Thread.Sleep(10);
                index++;
            }
            if (index >= timeout / 10) return false;
            byte[] buffer = { };
            serialPort.Read(buffer, 0, _retLength);
            serialPort.DiscardInBuffer();
            if (buffer.SequenceEqual(OkBuffer)) return true;
            else return false;
        }   
    
        //public WVISystemSettings ReadTestingParameter(string deviceport, string addr, int timeout = 1000)
        //{
        //    int _retLength = 5;

        //    byte[] ret = { };
        //    byte[] command = { 0x7B, 0x06, Convert.ToByte(addr), 0x04, 0x0A, 0x7D };
        //    SerialPort serialPort = new SerialPort(deviceport, 9600, Parity.None, 8, StopBits.One);
        //    serialPort.Open();
        //    serialPort.Write(command, 0, command.Length);
        //    int index = 0;
        //    while (index > timeout / 50 || serialPort.BytesToRead >= _retLength)
        //    {
        //        Thread.Sleep(50);
        //        index++;
        //    }
        //    if (index >= timeout / 10) return WVISystemSettings.R_PLC;


        //}
    
        //public 
    }
}
