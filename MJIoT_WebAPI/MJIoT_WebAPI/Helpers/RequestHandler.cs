using MJIoT_DBModel;
using MJIoT_WebAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MJIoT.Storage.PropertyValues;

namespace MJIoT_WebAPI.Helpers
{
    public class RequestHandler
    {
        IPropertyValuesStorage _propertyStorage;
        IModelStorage _modelStorage;
        IoTHubService iotHubServices;

        string BadUserMessage = "You do not have access to MJ IoT System! (User not recognized)";
        string PropertyNonExistentMessage = "This property does not exist in the system nad cannot be changed!";

        public RequestHandler(IModelStorage modelStorage, IPropertyValuesStorage propertyStorage)
        {
            _modelStorage = modelStorage;
            _propertyStorage = propertyStorage;
        }

        public int? DoUserCheck(string login, string password)
        {
            var userId = _modelStorage.GetUserId(login, password);
            if (userId == null)
                ThrowUnauthorizedResponse();

            return userId;
        }
   
        public async Task<List<DeviceDTO>> GetDevices(GetDevicesParams parameters)
        {
            var userId = DoUserCheck(parameters.User, parameters.Password);
            var devices = _modelStorage.GetDevicesOfUser(userId);

            iotHubServices = new IoTHubService();

            List<DeviceDTO> result = new List<DeviceDTO>();
            foreach (var device in devices)
            {
                DeviceDTO deviceData = await GetDeviceDTO(device);
                result.Add(deviceData);
            }

            return result;
        }    

        private async Task<DeviceDTO> GetDeviceDTO(MJIoT_DBModel.Device device)
        {
            //var name = _modelStorage.GetDeviceName(device);
            var name = await _propertyStorage.GetPropertyValueAsync(device.Id, "Name");
            var isConnected = await iotHubServices.IsDeviceOnline(device.Id.ToString());
            var deviceRole = _modelStorage.GetDeviceRole(device);

            var connectedListeners = GenerateListenersData(device);

            var item = new DeviceDTO
            {
                Id = device.Id,
                Name = name,
                CommunicationType = deviceRole,
                IsConnected = isConnected,
                ConnectedListeners = connectedListeners
            };

            return item;
        }

        private List<PropertyListenersInfo> GenerateListenersData(MJIoT_DBModel.Device device)
        {
            var connections = _modelStorage.GetConnections(device);
            var connectionGroups = connections
                .GroupBy(n => n.SenderProperty.Name);

            var result = new List<PropertyListenersInfo>();
            foreach (var group in connectionGroups)
            {
                PropertyListenersInfo propertyListener = GeneratePropertyListenerInfo(group);
                result.Add(propertyListener);
            }

            return result;
        }

        private PropertyListenersInfo GeneratePropertyListenerInfo(IGrouping<string, Connection> group)
        {
            var propertyListener = new PropertyListenersInfo
            {
                PropertyName = group.Key,
                Listeners = new List<SingleListenerInfo>()
            };

            foreach (var connection in group)
            {
                propertyListener.Listeners.Add(
                    new SingleListenerInfo
                    {
                        PropertyName = connection.ListenerProperty.Name,
                        DeviceId = connection.ListenerDevice.Id.ToString(),
                        Condition = connection.Condition,
                        ConditionValue = connection.ContitionValue
                    }
                );
            }

            return propertyListener;
        }

        private void ThrowUnauthorizedResponse()
        {
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = new StringContent(BadUserMessage)
            };
            throw new HttpResponseException(message);
        }
    }
}