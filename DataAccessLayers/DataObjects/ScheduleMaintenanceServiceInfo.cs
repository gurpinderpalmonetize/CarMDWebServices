using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayers.DataObjects
{
    public class ScheduleMaintenanceServiceInfo
    {
        public int Mileage { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public FixInfo ServiceInfo { get; set; }
    }
}
