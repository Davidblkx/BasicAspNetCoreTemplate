namespace OwnAspNetCore.Services
{
    public interface ISecurity
    {
        byte[] Hash(string source);
        string HashToString(string source);
        string HashToString(string source, string salt);
        string GenerateSalt();
        bool Validate(string hash, string password, string salt);
    }
}