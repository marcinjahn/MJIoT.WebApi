﻿using MJIoT_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MJIoT_WebAPI.Controllers
{
    public class DevicesController : ApiController
    {
        private MJIoT_DBModel.MJIoTDBContext context = new MJIoT_DBModel.MJIoTDBContext();

        public DevicesDTO GetDevices()
        {

            return null;
        }
    }
}
