using System.Text.Json.Serialization;

namespace Gyazo;

public interface IUsers
{
    Task<UserResponse> GetAsync(CancellationToken cancellationToken = default);
}

public record UserResponse
{
    [JsonPropertyName("user")]
    public User User { get; init; } = new();
}

public record User
{
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("profile_image")]
    public string? ProfileImage { get; init; }

    [JsonPropertyName("uid")]
    public string? UserId { get; init; }
}