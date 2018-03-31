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
using MJIoT.Storage.Models;
using MJIoT_WebAPI.Models;

namespace MJIoT_WebAPI.Tests
{
    [TestClass]
    public class TestDevicesController
    {
        [TestMethod]
        public void SetListenersTest()
        {
            var handler = new RequestHandler(new UnitOfWork(), new DocumentDbRepository());

            var parameters = new SetListenersParams
            {
                SenderId = 7,
                SenderPropertyId = 5,
                Listeners = new List<ListenerData>
                {
                    new ListenerData
                    {
                        DeviceId = 8,
                        PropertyId = 3,
                        Condition = ConnectionConditionTypes.NoCondition,
                        ConditionValue = null
                    },
                    new ListenerData
                    {
                        DeviceId = 16,
                        PropertyId = 6,
                        Condition = ConnectionConditionTypes.NoCondition,
                        ConditionValue = null,
                    }
                },
                User = "user1",
                Password = "pass1"
            };
            handler.SetListeners(parameters);
        }

        [TestMethod]
        public void Test1()
        {
            var unitOfWork = new UnitOfWork();

            var test = unitOfWork.Devices.Get(7);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task GetPropertiesTest()
        {
            var handler = new RequestHandler(new UnitOfWork(), new DocumentDbRepository());
            var result = await handler.GetProperties(new Models.GetPropertiesParams { User = "user1", Password = "pass1", DeviceId = "18" });

            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task GetAllDevicesForUser()
        {
            //var controller = new DevicesController();

            //var result = await controller.GetDevices(new Models.GetDevicesParams { User = "user1", Password="pass1" });

            var handler = new RequestHandler(new UnitOfWork(), new DocumentDbRepository());
            var result = await handler.GetDevices(new Models.GetDevicesParams { User = "user1", Password = "pass1" }, true);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestGeneratingListeners()
        {
            var context = new MJIoTDBContext();
            var device = context.Devices.Where(n => n.Id == 7).FirstOrDefault();

            var handler = new RequestHandler(new UnitOfWork(), new DocumentDbRepository());

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
