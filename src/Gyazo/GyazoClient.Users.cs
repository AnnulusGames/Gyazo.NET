using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Gyazo;

public partial class GyazoClient : IUsers
{
    public IUsers Users => this;

    async Task<UserResponse> IUsers.GetAsync(CancellationToken cancellationToken)
    {
        var requestUri = ApiEndpoints.Users;
        if (HttpClient.BaseAddress != null)
        {
            requestUri = new Uri("users/me", UriKind.Relative);
        }

        var message = new HttpRequestMessage(HttpMethod.Get, requestUri);
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

        var response = await httpClient.SendAsync(message, cancellationToken)
            .ConfigureAwait(ConfigureAwait);

        switch ((int)response.StatusCode)
        {
            case 200:
#if NET6_0_OR_GREATER
                var result = await response.Content.ReadFromJsonAsync<UserResponse>(GyazoJsonSerializerContext.Default.Options, cancellationToken)
                    .ConfigureAwait(ConfigureAwait);
#else
                var result = JsonSerializer.Deserialize<UserResponse>(await response.Content.ReadAsByteArrayAsync().ConfigureAwait(ConfigureAwait), GyazoJsonSerializerContext.Default.Options);
#endif
                return result!;
            default:
                throw await CreateApiException(response, ConfigureAwait, cancellationToken);
        }
    }
}