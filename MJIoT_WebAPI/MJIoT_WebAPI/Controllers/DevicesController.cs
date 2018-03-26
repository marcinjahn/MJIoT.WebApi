using MJIoT_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using MJIoT_WebAPI.Helpers;

namespace MJIoT_WebAPI.Controllers
{
    public class DevicesController : ApiController
    {
        private RequestHandler _handler;

        public DevicesController()
        {
            _handler = new RequestHandler(new ModelStorageSQL());
        }

        [HttpPost]
        [ActionName("GetDevices")]
        public async Task<List<DeviceDTO>> GetDevices(GetDevicesParams parameters)
        {
            var userCheck = _handler.DoUserCheck(parameters.User, parameters.Password);
            return await _handler.GetDevices(parameters);
        }

        //Sets listeners overwriting the previous set of listeners. Therefore the invoker should supply ALL listeners - newly added and previous ones that should stay.
        //listeners is a list of IDs.
        //[HttpPost]
        //[ActionName("SetListeners")]
        //public HttpResponseMessage SetListeners(SetListenersParams parameters)
        //{
        //    var userCheck = Helper.CheckUser(parameters.User, parameters.Password);

        //    if (userCheck == null)
        //    {
        //        HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Unauthorized)
        //        {
        //            Content = new StringContent(BadUserMessage)
        //        };
        //        throw new HttpResponseException(message);
        //    }

        //    using (var context = new MJIoTDBContext())
        //    {
        //        var sender = context.Devices
        //            .Include("ListenerDevices").Include("User")
        //            .Where(n => n.Id == parameters.SenderId && n.User.Id == userCheck)
        //            .FirstOrDefault();

        //        List<MJIoT_DBModel.Device> newListeners = new List<MJIoT_DBModel.Device>();

        //        if (parameters.Listeners != null) //Only iterate if list is not empty
        //        {
        //            foreach (var listener in parameters.Listeners)
        //            {
        //                var newListener = context.Devices
        //                    .Where(n => n.Id == listener)
        //                    .FirstOrDefault();

        //                newListeners.Add(newListener);
        //            }
        //        }

        //        //sender.ListenerDevices = newListeners;
        //        context.SaveChanges();
        //    }

        //    return new HttpResponseMessage(HttpStatusCode.OK);
        //}

        //[HttpPost]
        //[ActionName("SetProperty")]
        //public HttpResponseMessage SetProperty(SetPropertyParams parameters)
        //{
        //    var userCheck = Helper.CheckUser(parameters.User, parameters.Password);

        //    if (userCheck == null)
        //    {
        //        HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Unauthorized)
        //        {
        //            Content = new StringContent(BadUserMessage)
        //        };
        //        throw new HttpResponseException(message);
        //    }

        //    using (var context = new MJIoTDBContext())
        //    {
        //        var property = context.DeviceProperties
        //            .Include("Device")
        //            .Where(n => n.Device.Id == parameters.DeviceId && n.Id == parameters.PropertyId)
        //            .FirstOrDefault();

        //        if (property == null)
        //        {
        //            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.NotFound)
        //            {
        //                Content = new StringContent(PropertyNonExistentMessage)
        //            };
        //            throw new HttpResponseException(message);
        //        }

        //        property.Value = parameters.Value;
        //        context.SaveChanges();
        //    }
        //    return new HttpResponseMessage(HttpStatusCode.OK);
        //}

        //[HttpPost]
        //[ActionName("GetProperties")]
        //public List<PropertyDTO> GetProperties(GetPropertiesParams parameters)
        //{
        //    var userCheck = Helper.CheckUser(parameters.User, parameters.Password);

        //    if (userCheck == null)
        //    {
        //        HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Unauthorized)
        //        {
        //            Content = new StringContent(BadUserMessage)
        //        };
        //        throw new HttpResponseException(message);
        //    }

        //    List<PropertyDTO> result = new List<PropertyDTO>();

        //    using (var context = new MJIoTDBContext())
        //    {
        //        result = context.DeviceProperties
        //            .Include("Device").Include("PropertyType")
        //            .Where(n => n.Device.Id == parameters.DeviceId)
        //            .Select(n => new PropertyDTO
        //                {
        //                    Id = n.Id,
        //                    Value = n.Value,
        //                    Name = n.PropertyType.Name,
        //                    IsConfigurable = n.PropertyType.UIConfigurable
        //                })
        //            .ToList();
        //    }

        //    return result;
        //}
    }
}
