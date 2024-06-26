using System.Net.Http.Json;
using System.Text.Json;

namespace Gyazo;

public interface IGyazoClient
{
    public IImages Images { get; }
}

public sealed partial class GyazoClient : IGyazoClient, IDisposable
{
    public string AccessToken { get; init; } = Environment.GetEnvironmentVariable("GYAZO_TOKEN") ?? "";
    public bool ConfigureAwait { get; set; } = false;

    readonly HttpClient httpClient;
    public HttpClient HttpClient => httpClient;

    public GyazoClient(HttpMessageHandler handler, bool disposeHandler)
    {
        httpClient = new(handler, disposeHandler);
    }

    public GyazoClient()
        : this(new HttpClientHandler(), true)
    {
    }

    public GyazoClient(HttpMessageHandler handler)
        : this(handler, true)
    {
    }

    public void Dispose()
    {
        httpClient.Dispose();
    }

    Uri GetImageUri(string imageId)
    {
        Uri requestUri;
        if (HttpClient.BaseAddress == null)
        {
            requestUri = new Uri($"https://api.gyazo.com/api/images/{imageId}", UriKind.RelativeOrAbsolute);
        }
        else
        {
            requestUri = new Uri($"images/{imageId}", UriKind.Relative);
        }

        return requestUri;
    }

    async static Task<GyazoApiException> CreateApiException(HttpResponseMessage response, bool configureAwait, CancellationToken cancellationToken)
    {
        var contentType = response.Content.Headers.ContentType;
        var mediaType = contentType?.MediaType;
        if (mediaType != null && mediaType!.Equals("application/json", StringComparison.OrdinalIgnoreCase))
        {
#if NET6_0_OR_GREATER
            var result = await response.Content.ReadFromJsonAsync<ErrorResponse>(GyazoJsonSerializerContext.Default.Options, cancellationToken)
                .ConfigureAwait(configureAwait);
#else
            var result = JsonSerializer.Deserialize<ErrorResponse>(await response.Content.ReadAsByteArrayAsync().ConfigureAwait(configureAwait), GyazoJsonSerializerContext.Default.Options);
#endif

            return new GyazoApiException((ErrorCode)response.StatusCode, result!.Message);
        }
        
#if NET6_0_OR_GREATER
        return new GyazoApiException((ErrorCode)response.StatusCode, await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(configureAwait));
#else
        return new GyazoApiException((ErrorCode)response.StatusCode, await response.Content.ReadAsStringAsync().ConfigureAwait(configureAwait));
#endif
    }

    static class ApiEndpoints
    {
        public static readonly Uri Images = new("https://api.gyazo.com/api/images", UriKind.RelativeOrAbsolute);
        public static readonly Uri Upload = new("https://upload.gyazo.com/api/upload", UriKind.RelativeOrAbsolute);
        public static readonly Uri Users = new("https://api.gyazo.com/api/users/me", UriKind.RelativeOrAbsolute);
    }
}