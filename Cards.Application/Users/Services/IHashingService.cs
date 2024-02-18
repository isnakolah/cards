namespace Cards.Application.Users.Services;

public interface IHashingService
{
    bool Verify(string value, string hash);
    string Hash(string value);
}