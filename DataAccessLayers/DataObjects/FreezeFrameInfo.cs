using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayers.DataObjects
{
    public class FreezeFrameInfo
    {
        public FreezeFrameInfo()
        {
        }
        public string Description { get; set; }
        public string Value { get; set; }
        //protected internal static FreezeFrameInfo GetWebServiceObject(FreezeFrame sdkObject)
        //{
        //    FreezeFrameInfo wsObject = new FreezeFrameInfo();

        //    wsObject.Description = sdkObject.Description;
        //    wsObject.Value = sdkObject.Value;

        //    return wsObject;
        //}

    }
}
