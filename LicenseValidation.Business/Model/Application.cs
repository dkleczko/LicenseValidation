namespace LicenseValidation.Business.Model
{
    public class Application
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public int TrialDays { get; set; }

        public bool IsTrial()
        {
            return TrialDays > 0;
        }
    }
}
