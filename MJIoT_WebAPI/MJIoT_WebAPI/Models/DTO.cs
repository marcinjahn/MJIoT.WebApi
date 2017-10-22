using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MJIoT_WebAPI.Models
{

    //informacja na temat urządzeń przesyłana do aplikacji mobilnej
    public class DevicesDTO
    {
        public List<DeviceDTO> Devices { get; set; }
    }

    public class DeviceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DeviceCommunicationType CommunicationType { get; set; }  //sender lub listener lub bidirectional
        public bool IsConnected { get; set; }
        public List<string> ConnectedListeners { get; set; } //zawiera nazwy urządzeń
        public List<string> ConnectedSenders { get; set; } //zawiera nazwy urządzeń
        //przydałby się jeszcze obrazek
    }
}