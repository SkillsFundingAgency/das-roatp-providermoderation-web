namespace SFA.DAS.Roatp.ProviderModeration.Web.Models
{
    public class ProviderDescriptionSubmitModel 
    {
        public int Ukprn { get; set; }
        public string LegalName { get; set; }
        public string ProviderDescription { get; set; }
        public string ExistingProviderDescription { get; set; }
    }
}
