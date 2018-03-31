using MJIoT.Storage.Models.Enums;
using MJIoT_WebAPI.Helpers;
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

    public class DeviceWithListenersDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DeviceType { get; set; }
        public DeviceRole CommunicationType { get; set; }  //sender lub listener lub bi-directional
        public bool IsConnected { get; set; }
        public List<PropertyListenersInfo> ConnectedListeners { get; set; } //zawiera nazwy urządzeń
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
        public bool IsConfigurable { get; set; }
        public string PropertyValue { get; set; }
        public bool IsListenerProperty { get; set; }
        public bool IsSenderProperty { get; set; }
    }
}