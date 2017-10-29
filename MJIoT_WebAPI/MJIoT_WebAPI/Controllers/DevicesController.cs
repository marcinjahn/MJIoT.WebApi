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
        [HttpPost]
        [ActionName("GetList")]
        public List<DeviceDTO> GetDevices(string user, string password)
        {
            List<DeviceDTO> result = new List<DeviceDTO>();

            using (var context = new MJIoTDBContext())
            {

                var userCheck = context.Users.Where(n => n.Login == user && n.Password == password).FirstOrDefault();
                if (userCheck == null)
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);  //mozna też dodac jakąś wiadomość do exception

                var devices = context.Devices
                    .Include("DeviceType.SenderProperty")
                    .Include("DeviceType.ListenerProperty")
                    .Where(n => n.User.Id == userCheck.Id).ToList();

                foreach (var device in devices)
                {
                    DeviceCommunicationType communicationType;

                    PropertyType senderProperty = device.DeviceType.SenderProperty;
                    PropertyType listenerProperty = device.DeviceType.ListenerProperty;
                    if (senderProperty != null && listenerProperty != null)
                        communicationType = DeviceCommunicationType.bidirectional;
                    else if (senderProperty == null && listenerProperty != null)
                        communicationType = DeviceCommunicationType.listener;
                    else
                        communicationType = DeviceCommunicationType.sender;
                   

                    var item = new DeviceDTO
                    {
                        Id = device.Id,
                        Name = context.DeviceProperties.Where(n => n.Device.Id == device.Id && n.PropertyType.Name == "DisplayName").FirstOrDefault().Value,
                        CommunicationType = communicationType,
                        IsConnected = context.DeviceProperties.Where(n => n.Device.Id == device.Id && n.PropertyType.Name == "IsConnected").FirstOrDefault().Value == "true" ? true : false,
                        ConnectedListeners = context.DeviceProperties.Where(n => n.PropertyType.Name == "Name" && n.Device.Id == device.Id).Select(n => n.Value).ToList()
                    };

                    result.Add(item);
                }
            }
            return result;
        }

        [HttpPost]
        [ActionName("SetListeners")]
        public void SetListeners()
        {

        }

        [HttpPost]
        [ActionName("SetProperty")]
        public void SetProperty()
        {

        }
    }
}
