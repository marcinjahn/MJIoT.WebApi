﻿using MJIoT_DBModel;
using MJIoT_WebAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MJIoT.Storage.PropertyValues;
using System;
using MJIoT.Storage.Models;

namespace MJIoT_WebAPI.Helpers
{
    public class RequestHandler
    {
        IPropertyValuesStorage _propertyStorage;
        IUnitOfWork _unitOfWork;
        IoTHubDeviceAvailabilityService iotHubServices;

        string BadUserMessage = "You do not have access to MJ IoT System! (User not recognized)";
        string PropertyNonExistentMessage = "This property does not exist in the system nad cannot be changed!";

        public RequestHandler(IUnitOfWork unitOfWork, IPropertyValuesStorage propertyStorage)
        {
            _unitOfWork = unitOfWork;
            _propertyStorage = propertyStorage;
        }

        public User DoUserCheck(string login, string password)
        {
            var user = _unitOfWork.Users.Get(login, password);
            if (user == null)
                ThrowUnauthorizedResponse();

            return user;
        }

        public async Task<List<DeviceWithListenersDTO>> GetDevices(GetDevicesParams parameters, bool includeListeners)
        {
            var user = DoUserCheck(parameters.User, parameters.Password);
            var devices = _unitOfWork.Devices.GetDevicesOfUser(user.Id);

            iotHubServices = new IoTHubDeviceAvailabilityService();

            List<DeviceWithListenersDTO> result = new List<DeviceWithListenersDTO>();
            foreach (var device in devices)
            {
                DeviceWithListenersDTO deviceData = await GetDeviceDTO(device, includeListeners);
                result.Add(deviceData);
            }

            return result;
        }

        public async Task<List<PropertyDTO>> GetProperties(GetPropertiesParams parameters)
        {
            var deviceId = int.Parse(parameters.DeviceId);
            var deviceType = _unitOfWork.Devices.GetDeviceType(deviceId);
            var properties = _unitOfWork.PropertyTypes.GetPropertiesOfDevice(deviceType);

            List<PropertyDTO> result = new List<PropertyDTO>();
            foreach (var property in properties)
            {
                result.Add(new PropertyDTO
                {
                    Id = property.Id,
                    IsConfigurable = property.UIConfigurable,
                    Name = property.Name,
                    IsListenerProperty = property.IsListenerProperty,
                    IsSenderProperty = property.IsSenderProperty
                });
            }


            List<Task> tasks = new List<Task>();
            foreach (var entry in result)
            {
                tasks.Add(
                    Task.Run(async () =>
                    {
                        entry.PropertyValue =
                        await _propertyStorage.GetPropertyValueAsync(
                            int.Parse(parameters.DeviceId),
                            entry.Name);
                    })
                );
            }
            await Task.WhenAll(tasks);

            return result;

            //List<Task<string>> valuesTasks = new List<Task<string>>();
            //foreach (var property in properties)
            //{
            //    valuesTasks.Add(
            //        _propertyStorage.GetPropertyValueAsync(
            //            parameters.DeviceId, 
            //            property.Name)
            //    );
            //}


        }

        public void SetListeners(SetListenersParams parameters)
        {
            _unitOfWork.Connections.RemoveAll();

            foreach (var listener in parameters.Listeners)
            {
                _unitOfWork.Connections.Add(new Connection
                {
                    SenderDevice = _unitOfWork.Devices.Get(parameters.SenderId),
                    SenderProperty = _unitOfWork.PropertyTypes.Get(parameters.SenderPropertyId),
                    ListenerDevice = _unitOfWork.Devices.Get(listener.DeviceId),
                    ListenerProperty = _unitOfWork.PropertyTypes.Get(listener.PropertyId),
                    Condition = listener.Condition,
                    ContitionValue = listener.ConditionValue
                });
            }

            _unitOfWork.Save();
        }

        private async Task<DeviceWithListenersDTO> GetDeviceDTO(MJIoT_DBModel.Device device, bool includeListeners)
        {
            //var name = _modelStorage.GetDeviceName(device);
            var name = await _propertyStorage.GetPropertyValueAsync(device.Id, "Name");
            var isConnected = await iotHubServices.IsDeviceOnline(device.Id.ToString());
            var deviceRole = _unitOfWork.Devices.GetDeviceRole(device);
            var type = device.DeviceType.Name;
            var connectedListeners =  includeListeners ? GenerateListenersData(device) : null;

            var item = new DeviceWithListenersDTO
            {
                Id = device.Id,
                Name = name,
                DeviceType = type,
                CommunicationType = deviceRole,
                IsConnected = isConnected,
                ConnectedListeners = connectedListeners
            };

            return item;
        }

        private List<PropertyListenersInfo> GenerateListenersData(MJIoT_DBModel.Device device)
        {
            var connections = _unitOfWork.Connections.GetDeviceConnections(device);
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