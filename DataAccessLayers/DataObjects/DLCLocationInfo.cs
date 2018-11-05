using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayers.DataObjects
{
    public class DLCLocationInfo
    {

        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int? LocationNumber { get; set; }
        public string Access { get; set; }
        public string Comments { get; set; }
        public string ImageFileName { get; set; }
        public string ImageFileUrl { get; set; }
        public string ImageFileUrlSmall { get; set; }
        public bool IsValid { get; set; }
        public ValidationFailureModel[] ValidationFailures { get; set; }

    }
}
