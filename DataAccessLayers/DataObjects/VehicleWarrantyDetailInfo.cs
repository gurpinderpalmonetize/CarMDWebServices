using DataAccessLayers.DataBase;

namespace DataAccessLayers.DataObjects
{
    public class VehicleWarrantyDetailInfo
    {
        public string WarrantyTypeDescription { get; set; }
        public int WarrantyType { get; set; }
        public int? MaxYears { get; set; }
        public int? MaxMileage { get; set; }
        public string Notes { get; set; }
        public bool IsTransferable { get; set; }
        public string DescriptionFormatted { get; set; }
        protected internal static VehicleWarrantyDetailInfo GetWebServiceObject(VehicleWarrantyDetail sdkObject)
        {
            VehicleWarrantyDetailInfo wsObject = new VehicleWarrantyDetailInfo();
            //wsObject.WarrantyTypeDescription = sdkObject.Registry.GetEnumDescription(sdkObject.WarrantyType);
            //wsObject.WarrantyType = (int)sdkObject.WarrantyType;
            //if (!sdkObject.MaxYears.IsNull)
            //{
            //    wsObject.MaxYears = sdkObject.MaxYears.Value;
            //}
            //if (!sdkObject.MaxMileage.IsNull)
            //{
            //    wsObject.MaxMileage = sdkObject.MaxMileage.Value;
            //}
            //wsObject.Notes = sdkObject.Notes_Translated;
            //wsObject.IsTransferable = sdkObject.VehicleWarranty.IsTransferable;
            //wsObject.DescriptionFormatted = sdkObject.ToString();
            return wsObject;
        }
    }
}
