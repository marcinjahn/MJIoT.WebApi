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
        //[ActionName("getit")]
        //public IEnumerable<string> Get()
        //{
        //    return new[] { "first", "second" };
        //}

        [HttpPost]
        [ActionName("GetDevices")]
        public List<DeviceDTO> GetDevices(string user, string password)
        {
            List<DeviceDTO> result = new List<DeviceDTO>();

            using (var context = new MJIoTDBContext())
            {
                var userCheck = context.Users
                    .Where(n => n.Login == user && n.Password == password)
                    .FirstOrDefault();

                if (userCheck == null)
                {
                    HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent("You do not have access to MJ IoT System! (User not recognized)")
                    };
                    throw new HttpResponseException(message);
                }

                var devices = context.Devices
                    .Include("DeviceType.SenderProperty")
                    .Include("DeviceType.ListenerProperty")
                    .Include("ListenerDevices")
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


                    var name = context.DeviceProperties.Include("PropertyType")
                            .Where(n => n.Device.Id == device.Id && n.PropertyType.Name == "DisplayName")
                            .FirstOrDefault().Value;

                    var isConnected = context.DeviceProperties.Include("PropertyType")
                            .Where(n => n.Device.Id == device.Id && n.PropertyType.Name == "IsConnected")
                            .FirstOrDefault()
                            .Value == "true" ? true : false;


                    List<ListenerDTO> connectedListeners = new List<ListenerDTO>();
                    var listeners = device.ListenerDevices.ToList();

                    foreach (var listener in listeners)
                    {
                        var listenerDTO = new ListenerDTO
                        {
                            Id = listener.Id,
                            Name = context.DeviceProperties.Include("PropertyType")
                            .Where(n => n.Device.Id == device.Id && n.PropertyType.Name == "DisplayName")
                            .FirstOrDefault().Value
                        };

                        connectedListeners.Add(listenerDTO);
                    }

                    var item = new DeviceDTO
                    {
                        Id = device.Id,
                        Name = name,
                        CommunicationType = communicationType,
                        IsConnected = isConnected,
                        ConnectedListeners = connectedListeners
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
        public void SetProperty(int deviceId, string propertyName, string value)
        {

        }

        [HttpPost]
        [ActionName("GetProperty")]
        public string GetProperty(int deviceId, string propertyName)
        {
            return "test123";
        }

    }
}
