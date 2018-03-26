using MJIoT_DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MJIoT_WebAPI.Models;

namespace MJIoT_WebAPI.Helpers
{
    public interface IModelStorage
    {
        int? GetUserId(string user, string password);
        string GetDeviceName(MJIoT_DBModel.Device device);
        List<MJIoT_DBModel.Device> GetDevicesOfUser(int? userId);
        DeviceRole GetDeviceRole(MJIoT_DBModel.Device device);
        List<Connection> GetConnections(Device device);
    }


    public class ModelStorageSQL : IModelStorage
    {
        MJIoTDBContext _context;

        public ModelStorageSQL()
        {
            _context = new MJIoTDBContext();
        }

        public int? GetUserId(string user, string password)
        {
            var userCheck = _context.Users
                .Where(n => n.Login == user && n.Password == password)
                .FirstOrDefault();

            if (userCheck == null)
            {
                return null;
            }

            return userCheck.Id;
        }

        public string GetDeviceName(MJIoT_DBModel.Device device)
        {
            return _context.DeviceProperties.Include("PropertyType")
                            .Where(n => n.Device.Id == device.Id && n.PropertyType.Name == "DisplayName")
                            .FirstOrDefault().Value;
        }


        public List<MJIoT_DBModel.Device> GetDevicesOfUser(int? userId)
        {
            return _context.Devices.Include(n => n.DeviceType)
                .Where(n => n.User.Id == userId).ToList();
        }

        public DeviceRole GetDeviceRole(MJIoT_DBModel.Device device)
        {
            var isSender = _context.PropertyTypes.Where(n => n.DeviceType.Id == device.DeviceType.Id).Any(n => n.IsSenderProperty);
            var isListener = _context.PropertyTypes.Where(n => n.DeviceType.Id == device.DeviceType.Id).Any(n => n.IsListenerProperty);

            if (isSender && isListener)
                return DeviceRole.bidirectional;
            else if (isSender)
                return DeviceRole.sender;
            else if (isListener)
                return DeviceRole.listener;
            else
                return DeviceRole.none;
        }

        public List<Connection> GetConnections(Device device)
        {
            return _context.Connections
                .Include(n => n.ListenerDevice)
                .Include(n => n.SenderProperty)
                .Include(n => n.ListenerProperty)
                .Where(n => n.SenderDevice.Id == device.Id)
                .ToList();
        }

    }
}