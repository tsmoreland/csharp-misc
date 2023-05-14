//
// Copyright (c) 2022 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;

string root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));

string certificatePemFile = Path.Combine(root, "certificate.cer");
string certificateKeyFile = Path.Combine(root, "certificate.key");
string certificatePfxFile = Path.Combine(root, "certificate.pfx");

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddUserSecrets("3c6140af-715e-4445-841f-7705e4474291")
    .Build();

string? password = configuration["password"];

/* This doesn't work, for due to some issue within HttpClientHandler, for whatever reason swapping this
   to load the PFX file worked, while PEM and Key did not
X509Certificate2 certificate = X509Certificate2.CreateFromEncryptedPemFile(
    certificatePemFile,
    password,
    certificateKeyFile);
*/
X509Certificate2 certificate = new(certificatePfxFile, password);

using HttpClient client = new(new HttpClientHandler
{
    ClientCertificateOptions = ClientCertificateOption.Manual,
    SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
    ClientCertificates = { certificate },
    ServerCertificateCustomValidationCallback = (_, cert, _, _) => cert is not null,
});

// see CertificateAuth.App launch settings
client.BaseAddress = new Uri("https://localhost:7141");

Console.WriteLine("Press enter to continue");
Console.ReadLine();

try
{
    HttpResponseMessage response = await client.GetAsync(@"api/hello?reflectedMessage=""hello""");

    if (!response.IsSuccessStatusCode)
    {
        Console.WriteLine($"Unsuccessful {response.StatusCode}");

        HttpRequestMessage request = new()
        {
            RequestUri = new Uri(@"api/hello?reflectedMessage=""hello""", UriKind.Relative),
            Method = HttpMethod.Get,
        };
        request.Headers.Add("X-APR-ClientCert", certificate.GetRawCertDataString());
        response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Unsuccessful with APR header: {response.StatusCode}");
            Console.ReadLine();
            return;
        }
    }

    string message = await response.Content.ReadAsStringAsync();
    Console.WriteLine($"Response '{message}' ({response.StatusCode})");
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

Console.ReadLine();
