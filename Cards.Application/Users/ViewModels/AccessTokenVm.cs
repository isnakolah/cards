namespace Cards.Application.Users.ViewModels;

public record AccessTokenVm
{
    public string Token { get; init; } = string.Empty;
    public DateTime Expires { get; init; }
    public UserVm User { get; init; } = default!;
    
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