using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Configuration;

[ExcludeFromCodeCoverage]
public class StaffAuthenticationConfiguration
{
    public string WtRealm { get; set; }
    public string MetadataAddress { get; set; }
}
