using Microsoft.Azure.Devices;
using MJIoT_DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MJIoT_WebAPI.Helpers
{
    public class Helper
    {
        static string connectionString = "HostName=MJIoT-Hub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=SzQKdF1y6bAEgGfZei2bmq1Jd83odc+B2x197n2MtxA=";

        public static int? CheckUser(string user, string password)
        {
            User userCheck;
            using (var context = new MJIoTDBContext())
            {
                userCheck = context.Users
                    .Where(n => n.Login == user && n.Password == password)
                    .FirstOrDefault();
            }

            if (userCheck == null)
            {
                return null;
            }

            return userCheck.Id;
        }

        public static async Task<List<Boolean>> AreDevicesOnline(List<string> ids)
        {
            var serviceClient = ServiceClient.CreateFromConnectionString(connectionString);

            List<Task<Boolean>> results = new List<Task<bool>>();

            //Console.WriteLine(DateTime.Now.ToString());
            foreach (var id in ids)
            {
                results.Add(IsDeviceOnline(id, serviceClient));
            }

            await Task.WhenAll(results.ToArray());

            //Task.WaitAll(results.ToArray());
            //Console.WriteLine(DateTime.Now.ToString());

            //Console.WriteLine("RESULTS:");
            var statuses = new List<Boolean>();
            foreach (var result in results)
            {
               statuses.Add(result.Result);
            }
            //Console.WriteLine(res.Result);
            //Console.WriteLine("Press Enter to exit.");
            //Console.ReadLine();

            return statuses;
        }

        public static async Task<Boolean> IsDeviceOnline(string deviceId, ServiceClient serviceClient)
        {
            if (serviceClient == null)
                serviceClient = ServiceClient.CreateFromConnectionString(connectionString);

            var methodInvocation = new CloudToDeviceMethod("conn") { ResponseTimeout = TimeSpan.FromSeconds(5) };
            CloudToDeviceMethodResult response;
            try
            {
                response = await serviceClient.InvokeDeviceMethodAsync(deviceId, methodInvocation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine();
                return false;
            }

            return true;
        }
    }
}