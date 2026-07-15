using System;
using API.DTO;
using API.Entities;
using API.Interfaces;

namespace API.Extensions;

public static class AppUserExtension
{
    public static UserDto ToDTO(this AppUser user, ITokenService tokenService)
    {
        return new UserDto
        {
            ID = user.Id,
            DisplayName = user.DisplayName,
            Email = user.Email,
            Token = tokenService.CreateToken(user)
        };
    }
}