using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvaWeb.Controllers
{
    public class DataRepository
    {
        public ICollection<Models.Suscripcion> GetAllSuscriptions(int user)
        {
            ICollection<Models.Suscripcion> result = null;
            using (var a = new UvaWeb.Models.UvaContext())
            {
                result = a.Suscripcions.Where(x => x.UserID == user).ToList();
            }
            return result;
        }
    }
}
