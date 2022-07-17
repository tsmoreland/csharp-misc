//
// Copyright © 2022 Terry Moreland
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

using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using TSMoreland.Certificates;
using TSMoreland.Certificates.Extensions;

byte[] serialNumber = RandomNumberGenerator.GetBytes(20);

const string certificateAuth = "1.3.6.1.5.5.7.3.2";
ExtendedSignedCertificateSettings extendedSettings = new(
    "CN=localhost",
    2048,
    DateTime.Today.Subtract(TimeSpan.FromDays(1)),
    DateTime.Today.AddYears(1),
    Array.Empty<string>(),
    new[] { new Oid(certificateAuth) },
    serialNumber,
    X509KeyUsageFlags.NonRepudiation | X509KeyUsageFlags.DigitalSignature,
    false);


CertificateSettings settings = new(
    2048,
    DateTime.Today.Subtract(TimeSpan.FromDays(1)),
    DateTime.Today.AddYears(1),
    "CN=localhost");
SignedCertificateSettings signedCertSettings = new(
    2048,
    DateTime.Today.Subtract(TimeSpan.FromDays(1)),
    DateTime.Today.AddYears(1),
    "CN=localhost",
    serialNumber);

(_, X509Certificate2 root) = CertificateFactory.Build(extendedSettings);

string? password = Console.ReadLine();

(string certificate, string privateKey) = root.ToPemEncodedCertificateKeyPair(password);

File.WriteAllText("certificate.cer", certificate);
File.WriteAllText("certificate.key", privateKey);
