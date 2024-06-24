using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Gyazo;

public partial class GyazoClient : IImages
{
    public IImages Images => this;

    async Task<ImageResponse> IImages.GetAsync(ImageRequest request, CancellationToken cancellationToken)
    {
        var requestUri = GetImageUri(request.ImageId);

        var message = new HttpRequestMessage(HttpMethod.Get, requestUri);
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

        var response = await httpClient.SendAsync(message, cancellationToken)
            .ConfigureAwait(ConfigureAwait);

        switch ((int)response.StatusCode)
        {
            case 200:
#if NET6_0_OR_GREATER
                var result = await response.Content.ReadFromJsonAsync<ImageResponse>(GyazoJsonSerializerContext.Default.Options, cancellationToken)
                    .ConfigureAwait(ConfigureAwait);
#else
                var result = JsonSerializer.Deserialize<ImageResponse>(await response.Content.ReadAsByteArrayAsync().ConfigureAwait(ConfigureAwait), GyazoJsonSerializerContext.Default.Options);
#endif
                return result!;
            default:
                throw await CreateApiException(response, ConfigureAwait, cancellationToken);
        }
    }

    async Task<ImageResponse[]> IImages.ListAsync(ImageRequest request, CancellationToken cancellationToken)
    {
        var requestUri = ApiEndpoints.Images;
        if (HttpClient.BaseAddress != null)
        {
            requestUri = new Uri("upload", UriKind.Relative);
        }

        var message = new HttpRequestMessage(HttpMethod.Get, requestUri);
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

        var response = await httpClient.SendAsync(message, cancellationToken)
            .ConfigureAwait(ConfigureAwait);

        switch ((int)response.StatusCode)
        {
            case 200:
#if NET6_0_OR_GREATER
                var result = await response.Content.ReadFromJsonAsync<ImageResponse[]>(GyazoJsonSerializerContext.Default.Options, cancellationToken)
                    .ConfigureAwait(ConfigureAwait);
#else
                var result = JsonSerializer.Deserialize<ImageResponse[]>(await response.Content.ReadAsByteArrayAsync().ConfigureAwait(ConfigureAwait), GyazoJsonSerializerContext.Default.Options);
#endif
                return result!;
            default:
                throw await CreateApiException(response, ConfigureAwait, cancellationToken);
        }
    }

    async Task<UploadImageResponse> IImages.UploadAsync(UploadImageRequest request, CancellationToken cancellationToken)
    {
        var requestUri = ApiEndpoints.Upload;
        if (HttpClient.BaseAddress != null)
        {
            requestUri = new Uri("upload", UriKind.Relative);
        }

        var message = new HttpRequestMessage(HttpMethod.Post, requestUri);
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

        var content = new MultipartFormDataContent
        {
            { new ByteArrayContent(request.ImageData), "imagedata", "imagedata" },
        };

        if (request.AccessPolicy != null) content.Add(new StringContent(request.AccessPolicy.Value.ToString()), "access_policy");
        if (request.MetadataIsPublic != null) content.Add(new StringContent(request.MetadataIsPublic.Value.ToString()), "metadata_is_public");
        if (request.RefererUrl != null) content.Add(new StringContent(request.RefererUrl), "referer_url");
        if (request.App != null) content.Add(new StringContent(request.App), "app");
        if (request.Title != null) content.Add(new StringContent(request.Title), "title");
        if (request.Description != null) content.Add(new StringContent(request.Description), "desc");
        if (request.CreatedAt != null) content.Add(new StringContent(((DateTimeOffset)request.CreatedAt.Value).ToUnixTimeSeconds().ToString()), "created_at");
        if (request.CollectionId != null) content.Add(new StringContent(request.CollectionId), "collection_id");

        message.Content = content;

        var response = await httpClient.SendAsync(message, cancellationToken)
            .ConfigureAwait(ConfigureAwait);

        switch ((int)response.StatusCode)
        {
            case 200:
#if NET6_0_OR_GREATER
                var result = await response.Content.ReadFromJsonAsync<UploadImageResponse>(GyazoJsonSerializerContext.Default.Options, cancellationToken)
                    .ConfigureAwait(ConfigureAwait);
#else
                var result = JsonSerializer.Deserialize<UploadImageResponse>(await response.Content.ReadAsByteArrayAsync().ConfigureAwait(ConfigureAwait), GyazoJsonSerializerContext.Default.Options);
#endif
                return result!;
            default:
                throw await CreateApiException(response, ConfigureAwait, cancellationToken);
        }

    }

    async Task<DeleteImageResponse> IImages.DeleteAsync(DeleteImageRequest request, CancellationToken cancellationToken)
    {
        var requestUri = GetImageUri(request.ImageId);

        var message = new HttpRequestMessage(HttpMethod.Delete, requestUri);
        message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);

        var response = await httpClient.SendAsync(message, cancellationToken)
            .ConfigureAwait(ConfigureAwait);

        switch ((int)response.StatusCode)
        {
            case 200:
#if NET6_0_OR_GREATER
                var result = await response.Content.ReadFromJsonAsync<DeleteImageResponse>(GyazoJsonSerializerContext.Default.Options, cancellationToken)
                    .ConfigureAwait(ConfigureAwait);
#else
                var result = JsonSerializer.Deserialize<DeleteImageResponse>(await response.Content.ReadAsByteArrayAsync().ConfigureAwait(ConfigureAwait), GyazoJsonSerializerContext.Default.Options);
#endif
                return result!;
            default:
                throw await CreateApiException(response, ConfigureAwait, cancellationToken);
        }
    }

}