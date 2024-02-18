using System.Security.Cryptography;
using System.Text;
using Cards.Application.Users.Services;

namespace Cards.Infrastructure.Services;

internal sealed class HmacSha512HashingService : IHashingService
{
    public bool Verify(string value, string hash)
    {
        var parts = hash.Split('.');
        var hashBytes = Convert.FromBase64String(parts[0]);
        var saltBytes = Convert.FromBase64String(parts[1]);

        using var hmac = new HMACSHA512(saltBytes);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));

        return computedHash.SequenceEqual(hashBytes);
    }

    string IHashingService.Hash(string value)
    {
        using var hmac = new HMACSHA512();

        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
        var salt = hmac.Key;

        return $"{Convert.ToBase64String(hash)}.{Convert.ToBase64String(salt)}";
    }
}