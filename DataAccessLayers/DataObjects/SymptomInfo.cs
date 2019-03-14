using DataAccessLayers.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayers.DataObjects
{
    public class SymptomInfo
    {
        /// <summary>
        /// Default constructor for the symptom info class
        /// </summary>
        public SymptomInfo()
        {
        }

        /// <summary>
        /// The <see cref="Guid"/> id of the symptom.
        /// </summary>
        public Guid SymptomId;

        /// <summary>
        /// The <see cref="string"/> type of the symptom.
        /// </summary>
        public string Type;

        /// <summary>
        /// The <see cref="string"/> observed event of the symptom.
        /// </summary>
        public string ObservedEvent;

        /// <summary>
        /// The <see cref="string"/> location of the symptom.
        /// </summary>
        public string Location;

        /// <summary>
        /// The <see cref="string"/> operational condition of the symptom.
        /// </summary>
        public string OperationalCondition;

        /// <summary>
        /// The <see cref="string"/> operational condition of the symptom.
        /// </summary>
        public string SurveyTechnicalInspection;

        /// <summary>
        /// The <see cref="string"/> name of the fix for this symptom.
        /// </summary>
        //public string FixName;


        /// <summary>
        /// Method updates the web service object from the supplied SDK object
        /// </summary>
        /// <param name="sdkObject"><see cref="Innova.Symptoms.Symptom"/> object to create the object from.</param>
        /// <param name="isPrimary"></param>
        /// <returns><see cref="FixInfo"/> object created from the supplied SDK object.</returns>
        protected internal static SymptomInfo GetWebServiceObject(Symptom sdkObject, bool isPrimary = false)
        {
            SymptomInfo wsObject = new SymptomInfo();

            //wsObject.SymptomId = sdkObject.Id;
            //wsObject.Type = sdkObject.SymptomFragmentType.SymptomFragmentValue;
            //wsObject.ObservedEvent = sdkObject.SymptomFragmentObservedEvent.SymptomFragmentValue;
            //wsObject.Location = sdkObject.SymptomFragmentLocation.SymptomFragmentValue;
            //wsObject.OperationalCondition = sdkObject.SymptomFragmentOperationalCondition.SymptomFragmentValue;
            //wsObject.OperationalCondition = sdkObject.SymptomFragmentSurveyTechnicalInspection.SymptomFragmentValue;
            //wsObject.OperationalCondition = sdkObject.SymptomFragmentFixAssistDescription.SymptomFragmentValue;
            return wsObject;
        }

    }
}
