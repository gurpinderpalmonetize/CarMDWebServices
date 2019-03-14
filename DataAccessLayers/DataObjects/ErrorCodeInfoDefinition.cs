namespace DataAccessLayers.DataObjects
{
    public class ErrorCodeInfoDefinition
    {
        public ErrorCodeInfoDefinition()
        {
        }
        public string Title { get; set; }
        public string LaymansTermsTitle { get; set; }
        public string LaymansTermDescription { get; set; }
        public string PossibleCauses { get; set; }
        public string Conditions { get; set; }
        public string LaymansTermsConditions { get; set; }
        public int LaymansTermSeverityLevel;
        public string LaymansTermSeverityLevelDefinition { get; set; }
        public string LaymansTermEffectOnVehicle { get; set; }
        public string LaymansTermResponsibleComponentOrSystem { get; set; }
        public string LaymansTermWhyItsImportant { get; set; }
        public string MessageIndicatorLampFile { get; set; }
        public string MessageIndicatorLampFileUrl { get; set; }
        public string MonitorFile { get; set; }
        public string MonitorFileUrl { get; set; }
        public string MonitorType { get; set; }
        public string PassiveAntiTheftIndicatorLampFile { get; set; }
        public string PassiveAntiTheftIndicatorLampFileUrl { get; set; }
        public string ServiceThrottleSoonIndicatorLampFile { get; set; }
        public string ServiceThrottleSoonIndicatorLampFileUrl { get; set; }
        public string TransmissionControlIndicatorLampFile { get; set; }
        public string TransmissionControlIndicatorLampFileUrl { get; set; }
        public int Trips { get; set; }
        public ErrorCodeInfoDefinitionVehicle[] ErrorCodeDefinitionVehicles { get; set; }

        //protected internal static ErrorCodeInfoDefinition GetWebServiceObject(DiagnosticReportResultErrorCodeDefinitionDisplay sdkObject, bool hasMultipleDefinitions)
        //{
        //    return GetWebServiceObject(sdkObject, hasMultipleDefinitions, true);
        //}
        /// <summary>
        /// Method updates the web service object from the supplied SDK object
        /// </summary>
        /// <param name="sdkObject"><see cref="DiagnosticReportResultErrorCodeDefinitionDisplay"/> object to create the object from.</param>
        /// <param name="hasMultipleDefinitions">A <see cref="bool"/> indicating whether or not there are multiple definitions.</param>
        /// <param name="includeLaymansTerms">A <see cref="bool"/> indicating whether or not to include laymans terms.</param>
        /// <returns><see cref="ErrorCodeInfoDefinition"/> object created from the supplied SDK object.</returns>
        //        protected internal static ErrorCodeInfoDefinition GetWebServiceObject(DiagnosticReportResultErrorCodeDefinitionDisplay sdkObject, bool hasMultipleDefinitions, bool includeLaymansTerms)
        //        {
        //            ErrorCodeInfoDefinition wsObject = new ErrorCodeInfoDefinition();

        //            wsObject.Title = XmlHelper.CleanInvalidXmlChars(sdkObject.Title_Translated); // Updated on 2017-11-06 by INNOVA Dev Team

        //#if (!AUTOZONE)
        //            wsObject.PossibleCauses = XmlHelper.CleanInvalidXmlChars(sdkObject.PossibleCauses_Translated); // Updated on 2017-11-06 by INNOVA Dev Team
        //            wsObject.Conditions = XmlHelper.CleanInvalidXmlChars(sdkObject.Conditions_Translated); // Updated on 2017-11-06 by INNOVA Dev Team


        //            wsObject.MessageIndicatorLampFile = sdkObject.MessageIndicatorLampFile;
        //            wsObject.MessageIndicatorLampFileUrl = Global.DtcInfoRootUrl + sdkObject.MessageIndicatorLampFile;
        //            wsObject.MonitorFile = sdkObject.MonitorFile;
        //            wsObject.MonitorFileUrl = Global.DtcInfoRootUrl + sdkObject.MonitorFile;
        //            wsObject.MonitorType = sdkObject.MonitorType;
        //            wsObject.PassiveAntiTheftIndicatorLampFile = sdkObject.PassiveAntiTheftIndicatorLampFile;
        //            wsObject.PassiveAntiTheftIndicatorLampFileUrl = Global.DtcInfoRootUrl + sdkObject.PassiveAntiTheftIndicatorLampFile;
        //            wsObject.ServiceThrottleSoonIndicatorLampFile = sdkObject.ServiceThrottleSoonIndicatorLampFile;
        //            wsObject.ServiceThrottleSoonIndicatorLampFileUrl = Global.DtcInfoRootUrl + sdkObject.ServiceThrottleSoonIndicatorLampFile;
        //            wsObject.TransmissionControlIndicatorLampFile = sdkObject.TransmissionControlIndicatorLampFile;
        //            wsObject.TransmissionControlIndicatorLampFileUrl = Global.DtcInfoRootUrl + sdkObject.TransmissionControlIndicatorLampFile;
        //            wsObject.Trips = sdkObject.Trips;
        //#endif
        //            //			sdkObject.MessageIndicatorLampFile

        //            if (includeLaymansTerms)
        //            {
        //                wsObject.LaymansTermsTitle = XmlHelper.CleanInvalidXmlChars(sdkObject.LaymansTermTitle_Translated); // Updated on 2017-11-06 by INNOVA Dev Team
        //                wsObject.LaymansTermDescription = XmlHelper.CleanInvalidXmlChars(sdkObject.LaymansTermDescription_Translated); // Updated on 2017-11-06 by INNOVA Dev Team
        //#if (!AUTOZONE)
        //                wsObject.LaymansTermsConditions = XmlHelper.CleanInvalidXmlChars(sdkObject.LaymansTermConditions_Translated); // Updated on 2017-11-06 by INNOVA Dev Team
        //                wsObject.LaymansTermSeverityLevel = sdkObject.LaymansTermSeverityLevel;
        //                wsObject.LaymansTermSeverityLevelDefinition = XmlHelper.CleanInvalidXmlChars(sdkObject.LaymansTermSeverityLevelDefinition_Translated); // Updated on 2017-11-06 by INNOVA Dev Team
        //                wsObject.LaymansTermEffectOnVehicle = XmlHelper.CleanInvalidXmlChars(sdkObject.LaymansTermEffectOnVehicle_Translated); // Updated on 2017-11-06 by INNOVA Dev Team
        //                wsObject.LaymansTermResponsibleComponentOrSystem = XmlHelper.CleanInvalidXmlChars(sdkObject.LaymansTermResponsibleComponentOrSystem_Translated); // Updated on 2017-12-14 by Nam Lu - INNOVA Dev Team
        //                wsObject.LaymansTermWhyItsImportant = XmlHelper.CleanInvalidXmlChars(sdkObject.LaymansTermWhyItsImportant_Translated); // Updated on 2017-12-14 by Nam Lu - INNOVA Dev Team
        //#endif
        //            }


        //            //if we have multiple definitions then we need to add the vehicles to the list
        //            //if (hasMultipleDefinitions)
        //            //{
        //            //    if (sdkObject.DiagnosticReportResultErrorCodeDefinitionDisplayVehicles != null && sdkObject.DiagnosticReportResultErrorCodeDefinitionDisplayVehicles.Count > 0)
        //            //    {
        //            //        set the vehicles here
        //            //        wsObject.ErrorCodeDefinitionVehicles = new ErrorCodeInfoDefinitionVehicle[sdkObject.DiagnosticReportResultErrorCodeDefinitionDisplayVehicles.Count];

        //            //        for (int i = 0; i < sdkObject.DiagnosticReportResultErrorCodeDefinitionDisplayVehicles.Count; i++)
        //            //        {

        //            //            DiagnosticReportResultErrorCodeDefinitionDisplayVehicle defv = sdkObject.DiagnosticReportResultErrorCodeDefinitionDisplayVehicles[i];

        //            //            ErrorCodeInfoDefinitionVehicle ecidv = new ErrorCodeInfoDefinitionVehicle();
        //            //            ecidv.BodyCode = defv.BodyCode;
        //            //            ecidv.EngineType = defv.EngineType;

        //            //            wsObject.ErrorCodeDefinitionVehicles[i] = ecidv;
        //            //        }
        //            //    }
        //            //    else
        //            //    {
        //            //        empty array
        //            //        wsObject.ErrorCodeDefinitionVehicles = new ErrorCodeInfoDefinitionVehicle[0];
        //            //    }
        //            //}
        //            return wsObject;

        //        }
    }
}
