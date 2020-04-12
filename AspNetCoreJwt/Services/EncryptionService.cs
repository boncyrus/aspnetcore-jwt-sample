using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Text;

namespace AspNetCoreJwt.Services
{
    public class EncryptionService : IEncryptionService
    {
        public string Encrypt(string data)
        {
            var fakeSalt = Encoding.ASCII.GetBytes("fake salt");

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: data,
                salt: fakeSalt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}