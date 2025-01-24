using backend.Extensions;
using backend.Interfaces.Services;
using backend.Models;
using backend.Models.Auth;

namespace backend.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly ITokenGenerator _tokenGenerator;

    public AuthenticationService(ITokenGenerator tokenGenerator)
    {
        _tokenGenerator = tokenGenerator;
    }

    public Task<AuthenticationsToken> Login(User user)
    {
        return _tokenGenerator.GenerateAccessToken(user);
    }
}