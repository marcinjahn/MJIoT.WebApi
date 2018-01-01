using MJIoT_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MJIoT_DBModel;
using MJIoT_WebAPI.Helpers;

namespace MJIoT_WebAPI.Controllers
{
    public class DevicesController : ApiController
    {
        //[ActionName("getit")]
        //public IEnumerable<string> Get()
        //{
        //    return new[] { "first", "second" };
        //}

        //MESSAGES USED TO RETURN ERRORS:
        private string BadUserMessage = "You do not have access to MJ IoT System! (User not recognized)";
        private string PropertyNonExistentMessage = "This property does not exist in the system nad cannot be changed!";

        public bool CheckUser(CheckUserParams parameters)
        {
            var userCheck = Helper.CheckUser(parameters.User, parameters.Password);

            if (userCheck != null)
                return true;
            else
                return false;
        }

        [HttpPost]
        [ActionName("GetDevices")]
        public List<DeviceDTO> GetDevices(GetDevicesParams parameters)
        {
            List<DeviceDTO> result = new List<DeviceDTO>();

            using (var context = new MJIoTDBContext())
            {
                var userCheck = Helper.CheckUser(parameters.User, parameters.Password);

                if (userCheck == null)
                {
                    HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent(BadUserMessage)
                    };
                    throw new HttpResponseException(message);
                }

                var devices = context.Devices
                    .Include("DeviceType.SenderProperty")
                    .Include("DeviceType.ListenerProperty")
                    .Include("ListenerDevices")
                    .Where(n => n.User.Id == userCheck).ToList();

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
                            .Where(n => n.Device.Id == listener.Id && n.PropertyType.Name == "DisplayName")
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

        //Sets listeners overwriting the previous set of listeners. Therefore the invoker should supply ALL listeners - newly added and previous ones that should stay.
        //listeners is a list of IDs.
        [HttpPost]
        [ActionName("SetListeners")]
        public HttpResponseMessage SetListeners(SetListenersParams parameters)
        {
            var userCheck = Helper.CheckUser(parameters.User, parameters.Password);

            if (userCheck == null)
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent(BadUserMessage)
                };
                throw new HttpResponseException(message);
            }

            using (var context = new MJIoTDBContext())
            {
                var sender = context.Devices
                    .Include("ListenerDevices").Include("User")
                    .Where(n => n.Id == parameters.SenderId && n.User.Id == userCheck)
                    .FirstOrDefault();

                List<MJIoT_DBModel.Device> newListeners = new List<MJIoT_DBModel.Device>();

                foreach (var listener in parameters.Listeners)
                {
                    var newListener = context.Devices
                        .Where(n => n.Id == listener)
                        .FirstOrDefault();

                    newListeners.Add(newListener);
                }

                sender.ListenerDevices = newListeners;
                context.SaveChanges();
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [ActionName("SetProperty")]
        public HttpResponseMessage SetProperty(SetPropertyParams parameters)
        {
            var userCheck = Helper.CheckUser(parameters.User, parameters.Password);

            if (userCheck == null)
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent(BadUserMessage)
                };
                throw new HttpResponseException(message);
            }

            using (var context = new MJIoTDBContext())
            {
                var property = context.DeviceProperties
                    .Include("Device")
                    .Where(n => n.Device.Id == parameters.DeviceId && n.Id == parameters.PropertyId)
                    .FirstOrDefault();

                if (property == null)
                {
                    HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(PropertyNonExistentMessage)
                    };
                    throw new HttpResponseException(message);
                }

                property.Value = parameters.Value;
                context.SaveChanges();
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [ActionName("GetProperties")]
        public List<PropertyDTO> GetProperties(GetPropertiesParams parameters)
        {
            var userCheck = Helper.CheckUser(parameters.User, parameters.Password);

            if (userCheck == null)
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent(BadUserMessage)
                };
                throw new HttpResponseException(message);
            }

            List<PropertyDTO> result = new List<PropertyDTO>();

            using (var context = new MJIoTDBContext())
            {
                result = context.DeviceProperties
                    .Include("Device").Include("PropertyType")
                    .Where(n => n.Device.Id == parameters.DeviceId)
                    .Select(n => new PropertyDTO
                        {
                            Id = n.Id,
                            Value = n.Value,
                            Name = n.PropertyType.Name,
                            IsConfigurable = n.PropertyType.UIConfigurable
                        })
                    .ToList();
            }

            return result;
        }

    }
}
