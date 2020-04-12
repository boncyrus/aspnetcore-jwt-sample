using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreJwt.Services
{
    public interface IEncryptionService
    {
        string Encrypt(string data);
    }
}