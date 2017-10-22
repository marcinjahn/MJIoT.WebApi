using MJIoT_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MJIoT_DBModel;

namespace MJIoT_WebAPI.Controllers
{
    public class DevicesController : ApiController
    {
        private MJIoT_DBModel.MJIoTDBContext context = new MJIoT_DBModel.MJIoTDBContext();


        public DevicesDTO GetDevices(string user, string password)
        {
            using (var context = new MJIoTDBContext())
            {
                var userCheck = context.Users.Where(n => n.Login == user && n.Password == password).FirstOrDefault();
                if (userCheck == null)
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);

                var devices = context.Devices.Where(n => n.User.Id == userCheck.Id).ToList();

                //trzeba stworzyć kolekcje DeviceDTO i wysłać spowrotem

            }
                return null;
        }
    }
}
