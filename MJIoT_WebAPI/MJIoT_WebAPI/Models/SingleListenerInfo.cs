using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MJIoT_WebAPI.Models
{
    public class SingleListenerInfo
    {
        public string DeviceId { get; set; }
        public string PropertyName { get; set; }
        public MJIoT_DBModel.ConnectionConditionTypes Condition { get; set; }
        public string ConditionValue { get; set; }
    }
}