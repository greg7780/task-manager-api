using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace TaskManagerApi.Handlers
{
    public class PasswordHashHandler
    {
        private static readonly RandomNumberGenerator _randomNumberGenerator = RandomNumberGenerator.Create();
        private const int _iterationCount = 10000;

        public static string HashPassword(string password)
        {
            int saltSize = 128 / 8;
            var salt = new byte[saltSize];
            _randomNumberGenerator.GetBytes(salt);

            var subkey = KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA512,
                _iterationCount,
                256 / 8
            );

            var outputBytes = new byte[1 + 4 + 4 + 4 + salt.Length + subkey.Length];
            outputBytes[0] = 0x01;
            WriteNetworkByteOrder(outputBytes, 1, (uint)KeyDerivationPrf.HMACSHA512);
            WriteNetworkByteOrder(outputBytes, 5, (uint)_iterationCount);
            WriteNetworkByteOrder(outputBytes, 9, (uint)saltSize);
            Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
            Buffer.BlockCopy(subkey, 0, outputBytes, 13 + saltSize, subkey.Length);

            return Convert.ToBase64String(outputBytes);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            var decoded = Convert.FromBase64String(hashedPassword);

            var prf = (KeyDerivationPrf)ReadNetworkByteOrder(decoded, 1);
            var iterationCount = (int)ReadNetworkByteOrder(decoded, 5);
            var saltLength = (int)ReadNetworkByteOrder(decoded, 9);

            var salt = new byte[saltLength];
            Buffer.BlockCopy(decoded, 13, salt, 0, salt.Length);

            var expectedSubkeyLength = decoded.Length - 13 - salt.Length;
            var expectedSubkey = new byte[expectedSubkeyLength];
            Buffer.BlockCopy(decoded, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

            var actualSubkey = KeyDerivation.Pbkdf2(providedPassword, salt, prf, iterationCount, expectedSubkeyLength);

            return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
        }

        private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value >> 0);
        }
        
        private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
        {
            return ((uint)(buffer[offset + 0]) << 24)
                 | ((uint)(buffer[offset + 1]) << 16)
                 | ((uint)(buffer[offset + 2]) << 8)
                 | buffer[offset + 3];
        }
    }
}
