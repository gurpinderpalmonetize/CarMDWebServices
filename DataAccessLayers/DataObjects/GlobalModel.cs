using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayers.DataObjects
{
    public class GlobalModel 
    {
        public static string PolkVehicleImageRootUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["PolkVehicleImageRootUrl"];
            }
        }
    }
}
