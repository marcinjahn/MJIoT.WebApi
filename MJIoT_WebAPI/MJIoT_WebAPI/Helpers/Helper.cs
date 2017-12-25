using MJIoT_DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MJIoT_WebAPI.Helpers
{
    public class Helper
    {
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
    }
}