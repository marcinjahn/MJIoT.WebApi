using MJIoT_DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MJIoT_WebAPI.Models
{
    public class UserParams
    {
        public string User { get; set; }
        public string Password { get; set; }
    }

    public class CheckUserParams
    {
    }

    public class GetDevicesParams
    {
    }



    public class ConfigureListenersParams
    {
        public int SenderId { get; set; }
        public int SenderPropertyId { get; set; }
        public IEnumerable<ListenerData> Listeners { get; set; }
    }

    public class SetPropertyParams
    {
        public string DeviceId { get; set; }
        public string PropertyId { get; set; }
        public string Value { get; set; }
    }

    public class GetPropertiesParams
    {
        public string DeviceId { get; set; }
    }

    public class GetDeviceListenersParams : GetPropertiesParams { }

    public class GetPropertyListenersParams : GetDeviceListenersParams
    {
        public string SenderPropertyName { get; set; }
    }


    public class ListenerData
    {
        public int DeviceId { get; set; }
        public int PropertyId { get; set; }
        public ConnectionConditionTypes Condition { get; set; }
        public string ConditionValue { get; set; }
    }
}