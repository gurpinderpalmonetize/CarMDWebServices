using System.Collections.Generic;

namespace DataAccessLayers.Common
{
    public class DtcCodeViewCollection : SmartCollectionBase
    {
        public DtcCodeViewCollection()
        {

        }
        public List<string> AllDtcs
        {
            get
            {
                List<string> dtcs = new List<string>();

                foreach (DtcCodeView dcv in this)
                {
                    foreach (string dtc in dcv.Codes)
                    {
                        if (!dtcs.Contains(dtc))
                        {
                            dtcs.Add(dtc);
                        }
                    }
                }

                return dtcs;
            }
        }
        public DtcCodeView this[int index]
        {
            get
            {
                return (DtcCodeView)List[index];
            }
        }
        public void Add(DtcCodeView value)
        {
            base.Add(value);
        }

        /// <summary>
        /// Removes a DtcCodeView from the collection.
        /// </summary>
        /// <param name="value">The DtcCodeView to be removed.</param>
        public void Remove(DtcCodeView value)
        {
            base.Remove(value);
        }

        /// <summary>
        /// Inserts a DtcCodeView into the collection.
        /// </summary>
        /// <param name="index">The location in the collection which to insert the value.</param>
        /// <param name="value">The value to be inserted.</param>
        public void Insert(int index, DtcCodeView value)
        {
            base.Insert(index, value);
        }
    }
}
