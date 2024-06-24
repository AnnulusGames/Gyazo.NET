# Gyazo.NET
 Gyazo API client for .NET and Unity.

[![NuGet](https://img.shields.io/nuget/v/Gyazo.svg)](https://www.nuget.org/packages/Gyazo)
[![Releases](https://img.shields.io/github/release/AnnulusGames/Gyazo.NET.svg)](https://github.com/AnnulusGames/Gyazo.NET/releases)
[![GitHub license](https://img.shields.io/github/license/AnnulusGames/Gyazo.NET.svg)](./LICENSE)

[English]((./README.md)) | 日本語

## 概要

Gyazo.NETは.NET向けの[Gyazo API](https://gyazo.com/api?lang=ja)のクライアントSDKです。

## インストール

Gyazo.NETはNuGetで配布されています。.NET Standard2.1、.NET6.0、.NET8.0をサポートします。

### .NET CLI
```
dotnet add package Gyazo
```

### Package Manager

```
Install-Package Gyazo
```

また、Gyazo.NETはUnityでも利用可能です。詳細は[Unity](#unity)のセクションを参照してください。

## 使い方

`GyazoClient`を用いてGyazo APIの呼び出しを行うことが可能です。

```cs
using System.IO;
using Gyazo;

using var client = new GyazoClient
{
    AccessToken = Environment.GetEnvironmentVariable("GYAZO_TOKEN") // デフォルト
};

// 画像ファイルを読み込み
var image = await File.ReadAllBytesAsync("image.png");

// 画像をアップロード
var response = await client.Images.UploadAsync(new()
{
    ImageData = image,
});

// 画像のURLを取得
Console.WriteLine(response.PermanentLinkUrl);
```

`GyazoClient`はImageおよびUsersのAPI呼び出しをサポートします。詳細は[公式のAPIリファレンス](https://gyazo.com/api/docs)を参照してください。

## 例外処理

APIの呼び出しに失敗した場合は`GyazoApiException`がスローされます。

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

## HttpClientのカスタマイズ

`GyazoClient`は標準の`HttpClient`を使用して通信を行います。HttpClientの挙動を調整したい場合には、作成時に`HttpClientHandler`を渡します。

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

Gyazo.NETはUnityで利用が可能です。Gyazo.NETをUnityに導入するには[NugetForUnity]を使用します。

1. NugetForUnityをインストールする
2. Nuget > Manage NuGet Packagesからウィンドウを開き、Gyazoを検索してインストール

WebGLなどの環境で使用する場合、通信層をUnityWebRequestに置き換える必要があります。以下は[UnityWebRequestHttpMessageHandler.cs](https://gist.github.com/neuecc/854192b8d176170caf2c53fa7589dc90)を使用した例です。

```cs
// HttpClientHandlerをUnityWebRequestHttpMessageHandlerに変更する
var client = new GyazoClient(new UnityWebRequestHttpMessageHandler())
{
    AccessToken = "YOUR_ACCESS_TOKEN",
    ConfigureAwait = true // WebGLで安全に動作させるためにConfigureAwaitをtrueに設定
};
```

## ライセンス

このライブラリはMITライセンスの下で配布されています。
