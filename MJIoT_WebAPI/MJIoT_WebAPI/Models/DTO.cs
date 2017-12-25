using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MJIoT_WebAPI.Models
{

    //informacja na temat urządzeń przesyłana do aplikacji mobilnej
    //public class DevicesDTO
    //{
    //    public List<DeviceDTO> Devices { get; set; }
    //}

    public class DeviceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DeviceCommunicationType CommunicationType { get; set; }  //sender lub listener lub bi-directional
        public bool IsConnected { get; set; }
        public List<ListenerDTO> ConnectedListeners { get; set; } //zawiera nazwy urządzeń
        //public List<string> ConnectedSenders { get; set; } //zawiera nazwy urządzeń   na razie to pomijam
        //przydałby się jeszcze obrazek
    }

    public class ListenerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class PropertyDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IcConfigurable { get; set; }
    }
}