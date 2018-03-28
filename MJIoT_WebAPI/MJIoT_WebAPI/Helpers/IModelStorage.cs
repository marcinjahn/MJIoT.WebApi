using MJIoT_DBModel;
using System.Collections.Generic;
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
        List<PropertyType> GetPropertiesOfDevice(int deviceId);
    }
}