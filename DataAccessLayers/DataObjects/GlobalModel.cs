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

        public static int DaysMasterTechHaveToProvideAFix
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["DaysMasterTechHaveToProvideAFix"]);
            }
        }

        public static string AmazonWebServicePrivateKey
        {
            get
            {
                return ConfigurationManager.AppSettings["AWSPrivateKey"];
            }
        }

        public static string AmazonWebServicePrivateKeyPath
        {
            get
            {
                return ConfigurationManager.AppSettings["AWSPrivateKeyPath"];
            }
        }

        public static string DlcImageRootUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["DlcImageRootUrl"];
            }
        }
    }
}
