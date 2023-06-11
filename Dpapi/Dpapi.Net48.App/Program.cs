using System;
using System.Security.Cryptography;
using System.Text;

namespace Dpapi.Net48.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine() ?? string.Empty;

            byte[] buffer = Encoding.UTF8.GetBytes(input);

            byte[] entropy = CreateRandomEntropy();
            byte[] encrypted = ProtectedData.Protect(buffer, entropy, DataProtectionScope.LocalMachine);
            string encoded = $"{Convert.ToBase64String(encrypted)}.{Convert.ToBase64String(entropy)}";

            Console.WriteLine(encoded);

            byte[] decrypted = ProtectedData.Unprotect(encrypted, entropy, DataProtectionScope.LocalMachine);

            string value = Encoding.UTF8.GetString(decrypted);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(value);
        }

        private static byte[] CreateRandomEntropy()
        {
            byte[] entropy = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(entropy);
            return entropy;
        }
    }
}
