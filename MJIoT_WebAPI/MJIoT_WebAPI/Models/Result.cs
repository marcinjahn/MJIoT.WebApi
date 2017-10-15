using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MJIoT_WebAPI.Models
{
    public class DevicesResult
    {
        public List<DeviceResult> Devices { get; set; }
    }

    public class DeviceResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }  //sender lub listener lub bidirectional
        public bool IsConnected { get; set; }
        public List<string> ConnectedDevices { get; set; }
        //przydałby się jeszcze obrazek
    }
}