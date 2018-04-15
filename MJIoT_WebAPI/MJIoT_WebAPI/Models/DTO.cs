using MJIoT.Storage.Models.Enums;
using MJIoT_DBModel;
using MJIoT_WebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MJIoT_WebAPI.Models
{

    public class DeviceWithListenersDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DeviceType { get; set; }
        public DeviceRole CommunicationType { get; set; }  //sender lub listener lub bi-directional
        public bool IsConnected { get; set; }
        public List<PropertyListenersDTO> ConnectedListeners { get; set; } //zawiera nazwy urządzeń
        //public List<string> ConnectedSenders { get; set; } //zawiera nazwy urządzeń   na razie to pomijam
        //przydałby się jeszcze obrazek
    }

    public class SingleListenerDTO
    {
        public string DeviceId { get; set; }
        public string PropertyName { get; set; }
        public MJIoT_DBModel.ConnectionConditionTypes Condition { get; set; }
        public string ConditionValue { get; set; }
    }

    public class PropertyListenersDTO
    {
        public string PropertyName { get; set; }
        public List<SingleListenerDTO> Listeners { get; set; }
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