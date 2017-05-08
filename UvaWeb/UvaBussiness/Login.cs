using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvaData;

namespace UvaBussiness
{
    public class Login
    {
        public UvaModel.User LoginUser(string email, string password)
        {
            UvaModel.User result;
            using(var db = new UvaData.UvaContext()){

                result = db.Users.Where(x => x.UserID == 1).FirstOrDefault<UvaModel.User>();
            }
            return result;
        }
    }
}
