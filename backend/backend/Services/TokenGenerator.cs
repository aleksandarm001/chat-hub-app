using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Extensions;
using backend.Interfaces.Services;
using backend.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services;

public class TokenGenerator : ITokenGenerator
{
    private readonly IConfiguration _configuration;

    public TokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<AuthenticationsToken> GenerateAccessToken(User user)
    {
        var authenticationResponse = new AuthenticationsToken();

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("id", user.Id.ToString()),
            new("username", user.Username),
            new("email", user.Email),
            new(ClaimTypes.Role, user.Role)
        };

        var jwt = CreateToken(claims, 60 * 24);
        authenticationResponse.Id = user.Id;
        authenticationResponse.AccessToken = jwt;
        return authenticationResponse;
    }
    
    private string CreateToken(IEnumerable<Claim> claims, double expirationTimeInMinutes)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(expirationTimeInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        throw new NotImplementedException();
    }
}