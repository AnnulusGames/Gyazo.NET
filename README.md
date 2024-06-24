# Gyazo.NET
 Gyazo API client for .NET and Unity.

[![NuGet](https://img.shields.io/nuget/v/Gyazo.svg)](https://www.nuget.org/packages/Gyazo)
[![Releases](https://img.shields.io/github/release/AnnulusGames/Gyazo.NET.svg)](https://github.com/AnnulusGames/Gyazo.NET/releases)
[![GitHub license](https://img.shields.io/github/license/AnnulusGames/Gyazo.NET.svg)](./LICENSE)

English | [日本語](./README_JA.md)

## Overview

Gyazo.NET is a client SDK for the [Gyazo API](https://gyazo.com/api?lang=en) for .NET.

## Installation

Gyazo.NET is distributed via NuGet and supports .NET Standard 2.1, .NET 6.0, and .NET 8.0.

### .NET CLI

```
dotnet add package Gyazo
```

### Package Manager

```
Install-Package Gyazo
```

Additionally, Gyazo.NET can be used with Unity. See the [Unity](#unity) section for details.

## Usage

You can call the Gyazo API using `GyazoClient`.

```cs
using System.IO;
using Gyazo;

using var client = new GyazoClient
{
    AccessToken = Environment.GetEnvironmentVariable("GYAZO_TOKEN") // Default
};

// Read the image file
var image = await File.ReadAllBytesAsync("image.png");

// Upload the image
var response = await client.Images.UploadAsync(new()
{
    ImageData = image,
});

// Get the image URL
Console.WriteLine(response.PermanentLinkUrl);
```

`GyazoClient` supports API calls for both Image and Users. For more details, refer to the [official API reference](https://gyazo.com/api/docs).

## Exception Handling

If an API call fails, a `GyazoApiException` will be thrown.

```cs
try
{
    var response = await client.Images.GetAsync(new()
    {
        ImageId = "...",
    });
}
catch (GyazoApiException ex)
{
    Console.WriteLine((int)ex.Status); // 401 (ErrorCode.AuthenticationError)
    Console.WriteLine(ex.Message);     // You are not authorized.
}
```

## Customizing HttpClient

`GyazoClient` uses the standard `HttpClient` for communication. If you want to adjust the behavior of HttpClient, you can pass an `HttpClientHandler` during its creation.

```cs
public class GyazoClient : IDisposable
{
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
}
```

## Unity

Gyazo.NET can be used with Unity. To install Gyazo.NET in Unity, use [NugetForUnity].

1. Install NugetForUnity
2. Open the Nuget > Manage NuGet Packages window, search for Gyazo, and install it

When using it in environments like WebGL, you need to replace the communication layer with UnityWebRequest. Here is an example using [UnityWebRequestHttpMessageHandler.cs](https://gist.github.com/neuecc/854192b8d176170caf2c53fa7589dc90).

```cs
// Change HttpClientHandler to UnityWebRequestHttpMessageHandler
var client = new GyazoClient(new UnityWebRequestHttpMessageHandler())
{
    AccessToken = "YOUR_ACCESS_TOKEN",
    ConfigureAwait = true // Set ConfigureAwait to true for safe operation in WebGL
};
```

## License

This library is distributed under the MIT License.