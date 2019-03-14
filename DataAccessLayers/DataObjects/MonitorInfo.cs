namespace DataAccessLayers.DataObjects
{
    public class MonitorInfo
	{
		public MonitorInfo()
		{
        }
		public	string	Description { get; set; }
        public  string	Value { get; set; }
        //protected internal static MonitorInfo GetWebServiceObject(Monitor sdkObject)
        //{
        //	MonitorInfo wsObject = new MonitorInfo();

        //	wsObject.Description	= sdkObject.Description;
        //	wsObject.Value			= sdkObject.Value;

        //	return wsObject;
        //}

    }
}
