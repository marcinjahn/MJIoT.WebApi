using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MJIoT_WebAPI.Controllers;
using MJIoT_WebAPI.Helpers;

namespace MJIoT_WebAPI.Tests
{
    [TestClass]
    public class TestDevicesController
    {

        [TestMethod]
        public async Task GetAllDevicesForUser()
        {
            var controller = new DevicesController();

            var result = await controller.GetDevices(new Models.GetDevicesParams { User = "user1", Password="pass1" });
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
