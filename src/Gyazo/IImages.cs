using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Gyazo;

public interface IImages
{
    Task<ImageResponse> GetAsync(ImageRequest request, CancellationToken cancellationToken = default);
    Task<ImageResponse[]> ListAsync(ImageRequest request, CancellationToken cancellationToken = default);
    Task<UploadImageResponse> UploadAsync(UploadImageRequest request, CancellationToken cancellationToken = default);
    Task<DeleteImageResponse> DeleteAsync(DeleteImageRequest request, CancellationToken cancellationToken = default);
}

public record ImageRequest
{
    [JsonPropertyName("image_id")]
    public required string ImageId { get; init; }
}

public record ListImageRequest
{
    [JsonPropertyName("page")]
    public int Page { get; init; } = 1;

    [JsonPropertyName("per_page")]
    public int PerPage { get; init; } = 20;
}

public record ImageResponse
{
    [JsonPropertyName("image_id")]
    public string ImageId { get; init; } = "";

    [JsonPropertyName("parmalink_url")]
    public string? ParmanentLinkUrl { get; init; }

    [JsonPropertyName("thumb_url")]
    public string? ThumbnailUrl { get; init; }

    [JsonPropertyName("type")]
    public string? Type { get; init; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; init; }

    [JsonPropertyName("metadata")]
    public ImageMetadata Metadata { get; init; } = new();

    [JsonPropertyName("ocr")]
    public ImageOcr Ocr { get; init; } = new();
}

public record ImageMetadata
{
    [JsonPropertyName("app")]
    public string? App { get; init; }

    [JsonPropertyName("title")]
    public string? Title { get; init; }

    [JsonPropertyName("url")]
    public string? Url { get; init; }

    [JsonPropertyName("desc")]
    public string? Description { get; init; }
}

public record ImageOcr
{
    [JsonPropertyName("locale")]
    public string? Locale { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }
}

public record UploadImageRequest
{
    [JsonPropertyName("imagedata")]
    public required byte[] ImageData { get; init; }

    [JsonPropertyName("access_policy")]
    public AccessPolicy? AccessPolicy { get; init; }

    [JsonPropertyName("metadata_is_public")]
    public bool? MetadataIsPublic { get; init; } = false;

    [JsonPropertyName("referer_url")]
    public string? RefererUrl { get; init; }

    [JsonPropertyName("app")]
    public string? App { get; init; }

    [JsonPropertyName("title")]
    public string? Title { get; init; }

    [JsonPropertyName("desc")]
    public string? Description { get; init; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; init; }

    [JsonPropertyName("collection_id")]
    public string? CollectionId { get; init; }
}

public enum AccessPolicy
{
    [EnumMember(Value = "anyone")]
    Anyone,
    [EnumMember(Value = "only_me")]
    OnlyMe
}

public record UploadImageResponse
{
    [JsonPropertyName("image_id")]
    public string ImageId { get; init; } = "";

    [JsonPropertyName("permalink_url")]
    public string? PermanentLinkUrl { get; init; }

    [JsonPropertyName("thumb_url")]
    public string? ThumbnailUrl { get; init; }

    [JsonPropertyName("url")]
    public string? Url { get; init; }

    [JsonPropertyName("type")]
    public string Type { get; init; } = "";
}

public record DeleteImageRequest
{
    [JsonPropertyName("image_id")]
    public required string ImageId { get; init; }
}

public record DeleteImageResponse
{
    [JsonPropertyName("image_id")]
    public string? ImageId { get; init; }

    [JsonPropertyName("type")]
    public string? Type { get; init; }
}