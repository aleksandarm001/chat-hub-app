using backend.Extensions;
using backend.Models;

namespace backend.Interfaces.Services;

public interface ITokenGenerator
{
    Task<AuthenticationsToken> GenerateAccessToken(User user);
    string GenerateRefreshToken();
}