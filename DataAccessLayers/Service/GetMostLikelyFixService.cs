using DataAccessLayers.DataBase;
using DataAccessLayers.DataObjects;

namespace DataAccessLayers.Service
{
    public class GetMostLikelyFixService
    {
        public innovaEntities _innovaEntities;

        public GetMostLikelyFixService() { }
        public GetMostLikelyFixService(innovaEntities innovaEntities)
        {
            _innovaEntities = innovaEntities;
        }

        public DiagReportInfo GetMostLikelyFixForVehicle(VehicleRequest apiRequest)
        {
            return new DiagReportInfo();
        }
    }
}
