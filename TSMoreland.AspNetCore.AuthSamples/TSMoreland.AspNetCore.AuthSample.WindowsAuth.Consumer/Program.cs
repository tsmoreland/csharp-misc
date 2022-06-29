using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TSMoreland.AspNetCore.AuthSample.WindowsAuth.Consumer;

// using http only because I haven't configured SSL for HttpSys
Uri uri = new("http://localhost:5071/api/hello");


var credentialsCache = new CredentialCache
{
    {
        uri, "Negotiate",
        CredentialCache.DefaultNetworkCredentials
    }
};

HttpClientHandler handler = new() { Credentials = credentialsCache, PreAuthenticate = true, UseDefaultCredentials = true };

Console.WriteLine("Press any key to continue");
Console.ReadKey();

using HttpClient httpClient = new (handler) { Timeout = new TimeSpan(0, 0, 10) };

HttpResponseMessage response = await httpClient.GetAsync(uri);

response.EnsureSuccessStatusCode();

JsonSerializerOptions serializeOptions = new()
{
    PropertyNamingPolicy = SnakeCaseJsonNamingPolicy.Instance,
};

HelloMessage? message = await response.Content.ReadFromJsonAsync<HelloMessage>(serializeOptions);
if (message is null)
{
    Console.WriteLine("empty response");
    return;
}

Console.WriteLine(message.UserName);
