﻿using Microsoft.IdentityModel.Tokens;
using MiniECommerce.Gateway.YARP.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiniECommerce.Gateway.YARP.Services;

public sealed class JwtProvider(
    IConfiguration configuration)
{
    public string CreateToken(User user)
    {
        List<Claim> claims = new()
        {
            new Claim("UserId",user.Id.ToString()),
            new Claim("UserName",user.UserName)
        };


        SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
            (configuration.GetSection("JWT:SecretKey").Value ?? ""));

        SigningCredentials signingCredentials = new(symmetricSecurityKey,SecurityAlgorithms.HmacSha512);

        JwtSecurityToken jwtSecurityToken = new(
            issuer: configuration.GetSection("JWT:Issuer").Value,
            audience: configuration.GetSection("JWT:Audience").Value,
            claims: null,
            notBefore: DateTime.Now,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: signingCredentials);

        JwtSecurityTokenHandler handler = new();

        string token = handler.WriteToken(jwtSecurityToken);

        return token;
    }
}
