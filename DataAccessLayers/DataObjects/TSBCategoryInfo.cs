using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayers.DataObjects
{
    public class TSBCategoryInfo
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int? TSBCount { get; set; }
        //protected internal static TSBCategoryInfo GetWebServiceObject(TsbCategory sdkObject)
        //{
        //    TSBCategoryInfo wsObject = new TSBCategoryInfo();

        //    wsObject.Id = sdkObject.Id;
        //    wsObject.Description = sdkObject.Description;
        //    wsObject.TSBCount = sdkObject.Count;

        //    return wsObject;

        //}
    }
}
