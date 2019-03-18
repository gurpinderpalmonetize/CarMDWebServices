namespace DataAccessLayers.WebService
{
    public  class WebServiceKey
    {

        private string keyField;

        private string languageStringField;

        private string regionField;

        private System.Nullable<int> currencyField;

        private string marketStringField;

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        public string LanguageString
        {
            get
            {
                return this.languageStringField;
            }
            set
            {
                this.languageStringField = value;
            }
        }

        /// <remarks/>
        public string Region
        {
            get
            {
                return this.regionField;
            }
            set
            {
                this.regionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<int> Currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        public string MarketString
        {
            get
            {
                return this.marketStringField;
            }
            set
            {
                this.marketStringField = value;
            }
        }
    }
}
