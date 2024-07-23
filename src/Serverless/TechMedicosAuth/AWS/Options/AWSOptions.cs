namespace TechMedicosAuth.AWS.Options
{
    public class AWSOptions
    {
        public string Region { get; set; } = string.Empty;
        public string UserPoolId { get; set; } = string.Empty;
        public string UserPoolClientId { get; set; } = string.Empty;
        public string UserTechLanches { get; set; } = string.Empty;
        public string EmailDefault { get; set; } = string.Empty;
        public string PasswordDefault { get; set; } = string.Empty;
    }
}