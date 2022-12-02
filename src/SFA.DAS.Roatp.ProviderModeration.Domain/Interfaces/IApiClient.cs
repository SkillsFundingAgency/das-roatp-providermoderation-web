using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.ProviderModeration.Domain.Interfaces
{
    public interface IApiClient
    {
        Task<T> Get<T>(string uri);
        Task<HttpStatusCode> Post<T>(string uri, T model);
    }
}
