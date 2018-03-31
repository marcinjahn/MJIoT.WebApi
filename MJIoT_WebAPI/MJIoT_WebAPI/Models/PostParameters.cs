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

    public class CheckUserParams : UserParams
    {
    }

    public class GetDevicesParams : UserParams
    {
    }

    public class AddListenerParams : UserParams
    {
        public int SenderDeviceId { get; set; }
        public int SenderPropertyId { get; set; }
        public int ListenerDeviceId { get; set; }
        public int ListenePropertyId { get; set; }

        public ConnectionConditionTypes Condition { get; set; }
        public string CinditionValue { get; set; }
    }

    public class RemoveListenerData : AddListenerParams
    {
    }

    public class SetListenersParams : UserParams
    {
        public int SenderId { get; set; }
        public int SenderPropertyId { get; set; }
        public IEnumerable<ListenerData> Listeners { get; set; }
    }

    public class SetPropertyParams : UserParams
    {
        public string DeviceId { get; set; }
        public string PropertyId { get; set; }
        public string Value { get; set; }
    }

    public class GetPropertiesParams : UserParams
    {
        public string DeviceId { get; set; }
    }


    public class ListenerData
    {
        public int DeviceId { get; set; }
        public int PropertyId { get; set; }
        public ConnectionConditionTypes Condition { get; set; }
        public string ConditionValue { get; set; }
    }
}