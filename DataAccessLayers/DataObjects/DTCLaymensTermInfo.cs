using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayers.DataObjects
{
   public class DTCLaymensTermInfo
    {
        public string Title { get; set; }
        public string Make { get; set; }
        public string ErrorCode { get; set; }
        public int DTCCodeLaymanTermId { get; set; }
        public string Description { get; set; }
        public int SeverityLevel { get; set; }
        public string SeverityLevelDefinition { get; set; }
        public string EffectOnVehicle { get; set; }
        public string ResponsibleComponentOrSystem { get; set; }
        public string WhyItsImportant { get; set; }
        public bool IsValid { get; set; }
        public ValidationFailureModel[] ValidationFailures { get; set; }
    }
}
