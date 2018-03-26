using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MJIoT_WebAPI.Models
{
    public class PropertyListenersInfo
    {
        public string PropertyName { get; set; }
        public List<SingleListenerInfo> Listeners { get; set; }
    }
}