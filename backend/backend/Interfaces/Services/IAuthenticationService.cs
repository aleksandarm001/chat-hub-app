using backend.Extensions;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Interfaces.Services;

public interface IAuthenticationService
{
    Task<AuthenticationsToken> Login(User user);
}