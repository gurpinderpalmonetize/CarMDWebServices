using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayers.DataBase;
using Metafuse3.Security;

namespace DataAccessLayers.WebService
{
    public class ExternalSystemWebService
    {
        public static string k = "WSKey1";
        public Guid? keyGuid { get; set; }
        public int? FixPriority { get; set; }
        public string name { get; set; }
        public string partnerID { get; set; }
        public string imageNameSuffix { get; set; }
        public DateTime createdDateTimeUTC { get; set; }
        public DateTime updatedDateTimeUTC { get; set; }
        public bool isActive { get; set; }
        public string AdminUserUpdatedId { get; set; }
        public string AdminUserCreatedId { get; set; }
        public bool IsObjectLoaded { get; set; }
        public List<string> updatedFields { get; set; }

        public static ExternalSystemWebService GetActiveExternalSystemFromKey(string encrpytedKey)
         { 
            var externalSystem = new ExternalSystemWebService();
            Guid? id = DecryptExternalKeyStringToId(encrpytedKey);

            if (id.HasValue)
            {
                using(innovaEntities dbContext = new innovaEntities())
                {
                    var keyGuid = Convert.ToString(id);
                    var extIdList = dbContext.ExternalSystems.Where(x => x.KeyGuid == keyGuid && x.IsActive == true).FirstOrDefault();
                    if(extIdList != null)
                    {
                        externalSystem.keyGuid = new Guid(extIdList.KeyGuid);
                        externalSystem.FixPriority = extIdList.FixPriority;
                        externalSystem.name = extIdList.Name;
                        externalSystem.partnerID = extIdList.PartnerID;
                        externalSystem.imageNameSuffix = extIdList.ImageNameSuffix;
                        externalSystem.isActive = Convert.ToBoolean(extIdList.IsActive);
                        externalSystem.createdDateTimeUTC = Convert.ToDateTime(extIdList.CreatedDateTimeUTC);
                        externalSystem.updatedDateTimeUTC = Convert.ToDateTime(extIdList.UpdatedDateTimeUTC);
                        externalSystem.AdminUserUpdatedId = extIdList.AdminUserUpdatedId;
                        externalSystem.AdminUserCreatedId = extIdList.AdminUserCreatedId;
                        externalSystem.IsObjectLoaded = true;
                    }
                };
                
                return externalSystem;
            }
            return externalSystem;
        }

        private static Guid? DecryptExternalKeyStringToId(string keyStringToDecode)
        {
            DESEncryptor desEncryptor = new DESEncryptor();
            string g;
            try
            {
                keyStringToDecode = keyStringToDecode.Replace(" ", "+");
                g = desEncryptor.DecryptData(ExternalSystemWebService.k, keyStringToDecode);
            }
            catch(Exception ex)
            {
                g = (string)null;
            }
            Guid? nullable = new Guid?();
            if (!string.IsNullOrEmpty(g))
            {
                try
                {
                    nullable = new Guid?(new Guid(g));
                }
                catch
                {
                    nullable = new Guid?();
                }
            }
            return nullable;
        }

        public Guid? KeyGuid
        {
            get
            {
                return this.keyGuid;
            }
            set
            {
                if (!(value != this.keyGuid))
                    return;
                this.keyGuid = value;
            }
        }
    }
}
