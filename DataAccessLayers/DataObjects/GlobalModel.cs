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

        public static bool UsePolkData
        {
            get
            {
                return Convert.ToBoolean( ConfigurationManager.AppSettings["UsePolkData"]);
            }
        }
        public static bool MasterTechAssignPwrNoFixReports
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["MasterTechAssignPwrNoFixReports"]);
            }
        }
        public static bool MasterTechAssignObd1NoFixReports
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["MasterTechAssignObd1NoFixReports"]);
            }
        }
        public static bool MasterTechAssignAbsNoFixReports
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["MasterTechAssignAbsNoFixReports"]);
            }
        }
        public static bool MasterTechAssignSrsNoFixReports
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["MasterTechAssignSrsNoFixReports"]);
            }
        }
        public static string ScoreLetterGradeCuttoffsList
        {
            get
            {
                return ConfigurationManager.AppSettings["ScoreLetterGradeCuttoffsList"];
            }
        }

        public static string ArticleVideoFileBaseUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ArticleVideoFileBaseUrl"];
            }
        }

        public static string ResourcesBaseUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ResourcesBaseUrl"];
            }
        }

        public static string ArticleVideoThumbnailVirtualPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ArticleVideoThumbnailVirtualPath"];
            }
        }

        public static string ArticleVideoStreamingBaseUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ArticleVideoStreamingBaseUrl"];
            }
        }

        public static string ArticleImageFileVirtualPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ArticleImageFileVirtualPath"];
            }
        }

        public static string ArticleDocumentFileVirtualPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ArticleDocumentFileVirtualPath"];
            }
        }

        public static string ArticleMediaFileVirtualPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ArticleMediaFileVirtualPath"];
            }
        }

        public static string ArticleFlashFileVirtualPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ArticleFlashFileVirtualPath"];
            }
        }
    }
}
