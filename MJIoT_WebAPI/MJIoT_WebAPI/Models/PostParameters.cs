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

    public class GetDevicesParams : UserParams
    {
    }

    public class SetListenersParams : UserParams
    {
        public int SenderId { get; set; }
        public int[] Listeners { get; set; }
    }

    public class SetPropertyParams : UserParams
    {
        public int DeviceId { get; set; }
        public string PropertyId { get; set; }
        public string Value { get; set; }
    }

    public class GetPropertiesParams : UserParams
    {
        public int DeviceId { get; set; }
    }
}