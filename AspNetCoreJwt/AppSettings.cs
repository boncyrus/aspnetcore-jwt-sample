namespace AspNetCoreJwt
{
    public class AppSettings
    {
        public JwtSettings JwtSettings { get; set; }
    }

    public class JwtSettings
    {
        public string Secret { get; set; }
        public double Expiration { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }
}