using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MJIoT_WebAPI.Controllers;
using MJIoT_WebAPI.Helpers;

using MJIoT_DBModel;

using MJIoT.Storage.PropertyValues;

namespace MJIoT_WebAPI.Tests
{
    [TestClass]
    public class TestDevicesController
    {
        [TestMethod]
        public async Task GetPropertiesTest()
        {
            var handler = new RequestHandler(new ModelStorageSQL(), new DocumentDbRepository());
            var result = await handler.GetProperties(new Models.GetPropertiesParams { User = "user1", Password = "pass1", DeviceId = "18" });

            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task GetAllDevicesForUser()
        {
            //var controller = new DevicesController();

            //var result = await controller.GetDevices(new Models.GetDevicesParams { User = "user1", Password="pass1" });

            var handler = new RequestHandler(new ModelStorageSQL(), new DocumentDbRepository());
            var result = await handler.GetDevices(new Models.GetDevicesParams { User = "user1", Password = "pass1" }, true);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestGeneratingListeners()
        {
            var context = new MJIoTDBContext();
            var device = context.Devices.Where(n => n.Id == 7).FirstOrDefault();

            var handler = new RequestHandler(new ModelStorageSQL(), new DocumentDbRepository());

            PrivateObject obj = new PrivateObject(handler);
            obj.Invoke("GenerateListenersData", device);

            Assert.IsTrue(true);
        }

        //[TestMethod]
        //public async Task CheckConnection()
        //{
        //    var id = "16";
        //    var res = await Helper.IsDeviceOnline(id, null);
        //    Assert.IsFalse(res);
        //}

    }
}
