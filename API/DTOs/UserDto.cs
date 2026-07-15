using System;

namespace API.DTO;

public class UserDto
{
    public required string DisplayName {get; set;}
    public required string Email {get; set;}
    public required string ID {get; set;}
    public string? ImageUrl {get; set;}
    public required string Token {get; set;}
}