using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Roatp.ProviderModeration.Web.Configuration;

[ExcludeFromCodeCoverage]
public class ApplicationConfiguration
{
    public string EsfaAdminServicesBaseUrl { get; set; }
    public virtual bool UseDfeSignIn { get; set; }
    public virtual string DfESignInServiceHelpUrl { get; set; }
}
