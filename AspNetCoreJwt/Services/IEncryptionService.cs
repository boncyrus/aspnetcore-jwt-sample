namespace AspNetCoreJwt.Services
{
    public interface IEncryptionService
    {
        string Encrypt(string data);
    }
}