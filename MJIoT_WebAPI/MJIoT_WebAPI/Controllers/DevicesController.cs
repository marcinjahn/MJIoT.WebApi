using MJIoT_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using MJIoT_WebAPI.Helpers;

using MJIoT.Storage.PropertyValues;
using MJIoT.Storage.Models;

namespace MJIoT_WebAPI.Controllers
{
    //[Authorize]
    public class DevicesController : ApiController
    {
        private RequestHandler _handler;

        public DevicesController()
        {
            _handler = new RequestHandler(new UnitOfWork(), new DocumentDbRepository());
        }

        [HttpPost] 
        [ActionName("GetDevicesWithListeners")]
        public async Task<List<DeviceWithListenersDTO>> GetDevicesWithListeners(GetDevicesParams parameters)
        {
            //var userCheck = _handler.DoUserCheck(parameters.User, parameters.Password);
            return await _handler.GetDevices(GetUserId(), true);
        }

        [HttpPost]
        [ActionName("GetDevices")]
        public async Task<List<DeviceWithListenersDTO>> GetDevices(GetDevicesParams parameters)
        {
            //var userCheck = _handler.DoUserCheck(parameters.User, parameters.Password);
            return await _handler.GetDevices(GetUserId(), false);
        }

        [HttpPost]
        [ActionName("GetProperties")]
        public async Task<List<PropertyDTO>> GetProperties(GetPropertiesParams parameters)
        {
            return await _handler.GetProperties(GetUserId(), parameters);
        }

        [HttpPost]
        [ActionName("SetListeners")]
        public IHttpActionResult SetListeners(ConfigureListenersParams parameters)
        {
            _handler.SetListeners(GetUserId(), parameters);

            return Ok();
        }

        [HttpPost]
        [ActionName("AddListeners")]
        public IHttpActionResult AddListeners(ConfigureListenersParams parameters)
        {
            _handler.AddListeners(GetUserId(), parameters);

            return Ok();
        }

        [HttpPost]
        [ActionName("GetDeviceListeners")]
        public List<PropertyListenersDTO> GetListeners(GetDeviceListenersParams parameters)
        {
            return _handler.GetDeviceListeners(GetUserId(), int.Parse(parameters.DeviceId)) as List<PropertyListenersDTO>;
        }

        [HttpPost]
        [ActionName("RemoveListeners")]
        public IHttpActionResult RemoveListeners(ConfigureListenersParams parameters)
        {
            _handler.RemoveListeners(GetUserId(), parameters);

            return Ok();
        }

        private int GetUserId()
        {
            return int.Parse(Request.Properties["userId"].ToString());
        }

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
    }
}
