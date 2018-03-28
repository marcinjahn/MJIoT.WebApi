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

    public class SetListenerParams : UserParams
    {
        public int SenderDeviceId { get; set; }
        public int SenderPropertyId { get; set; }
        public int ListenerDeviceId { get; set; }
        public int ListenePropertyId { get; set; }
    }

    public class SetListenersParams : UserParams
    {
        public int SenderId { get; set; }
        public int[] Listeners { get; set; }
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
}