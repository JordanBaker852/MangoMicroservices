﻿namespace Mango.Web.Models.DTO
{
    public class LoginResponseDTO
    {
        public UserDTO User { get; set; } = new UserDTO();
        public string Token { get; set; } = string.Empty;
    }
}
