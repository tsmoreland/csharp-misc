using System.Security.Cryptography;
using System.Text;

string input = Console.ReadLine() ?? string.Empty;

string[] parts = input.Split('.'); // yeah yeah, we should verify it has a . before accessing parts[0] and parts[1]

byte[] encrypted = Convert.FromBase64String(parts[0]);
byte[] entropy = Convert.FromBase64String(parts[1]);

if (OperatingSystem.IsWindows())
{
    byte[] decrypted = ProtectedData.Unprotect(encrypted, entropy, DataProtectionScope.LocalMachine);
    string value = Encoding.UTF8.GetString(decrypted);
    Console.WriteLine(value);
}
