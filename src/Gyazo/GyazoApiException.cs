using System.Net;

namespace Gyazo;

public sealed class GyazoApiException(HttpStatusCode status, string message) : Exception(message)
{
    public HttpStatusCode Status { get; } = status;

    public override string ToString()
    {
        return $"{Status}: {Message}";
    }
}