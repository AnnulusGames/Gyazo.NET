using System.Text.Json.Serialization;

namespace Gyazo;

public sealed class GyazoApiException(ErrorCode status, string? message) : Exception(message)
{
    public ErrorCode Status { get; } = status;

    public override string ToString()
    {
        return $"{Status}: {Message}";
    }
}

public enum ErrorCode
{
    InvalidRequestError = 400,
    AuthenticationError = 401,
    PermissionError = 403,
    NotFoundError = 404,
    UnprocessableError = 422,
    RateLimitError = 429,
    ApiError = 500,
}

public record ErrorResponse
{
    [JsonPropertyName("message")]
    public string? Message { get; init; }

    [JsonPropertyName("request")]
    public string? Request { get; init; }

    [JsonPropertyName("method")]
    public string? Method { get; init; }
}