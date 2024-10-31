﻿namespace IllyrianAPI.Models.Auth
{
    public class Login
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Token { get; set; } // For reCAPTCHA
    }
}
    