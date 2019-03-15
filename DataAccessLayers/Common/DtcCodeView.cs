using System.Collections.Generic;

namespace DataAccessLayers.Common
{
    public class DtcCodeView
    {
        private string codeType { get; set; }
        private List<string> codes { get; set; }

        public DtcCodeView(string codeType)
        {
            this.codeType = codeType;
            this.codes = new List<string>();
        }

        public string CodeType
        {
            get
            {
                return this.codeType;
            }
        }

        public List<string> Codes
        {
            get
            {
                return this.codes;
            }
        }
    }
}
