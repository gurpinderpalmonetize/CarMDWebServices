namespace DataAccessLayers.DataObjects
{
    public class WebServiceSessionStatus
    {
        public WebServiceSessionStatus()
        {
            this.ValidationFailures = new ValidationFailure[0];
        }
        public ValidationFailure[] ValidationFailures = null;
        protected internal void AddValidationFailure(string code, string description)
        {
            ValidationFailure[] previous = this.ValidationFailures;

            this.ValidationFailures = new ValidationFailure[previous.Length + 1];

            for (int i = 0; i < previous.Length; i++)
            {
                this.ValidationFailures[i] = previous[i];
            }
            ValidationFailure vf = new ValidationFailure();
            vf.Code = code;
            vf.Description = description;
            //add the newest failure to the end of the list
            ValidationFailures[this.ValidationFailures.Length - 1] = vf;
        }
    }
}
