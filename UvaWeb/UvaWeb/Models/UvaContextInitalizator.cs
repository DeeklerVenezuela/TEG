using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvaWeb.Models
{
    public class UvaContextInitalizator : CreateDatabaseIfNotExists<UvaContext>
    {
        protected override void Seed(UvaContext context)
        {
            base.Seed(context);
        }
    }
}
