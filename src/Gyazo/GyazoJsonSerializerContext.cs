using System.Text.Json.Serialization;

namespace Gyazo;

[JsonSerializable(typeof(ImageRequest))]
[JsonSerializable(typeof(ListImageRequest))]
[JsonSerializable(typeof(ImageResponse))]
[JsonSerializable(typeof(ImageMetadata))]
[JsonSerializable(typeof(ImageOcr))]
[JsonSerializable(typeof(UploadImageRequest))]
[JsonSerializable(typeof(UploadImageResponse))]
[JsonSerializable(typeof(DeleteImageRequest))]
[JsonSerializable(typeof(DeleteImageResponse))]
[JsonSerializable(typeof(UserResponse))]
public sealed partial class GyazoJsonSerializerContext : JsonSerializerContext
{
}