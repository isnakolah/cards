namespace Cards.Application.Users.ViewModels;

public record AccessTokenVm
{
    public string Token { get; private init; } = string.Empty;
    public DateTime Expires { get; private init; }
    public UserVm User { get; private init; } = default!;
    
    public static AccessTokenVm Create(string token, DateTime expires, UserVm user)
    {
        return new AccessTokenVm
        {
            Token = token,
            Expires = expires,
            User = user
        };
    }
}