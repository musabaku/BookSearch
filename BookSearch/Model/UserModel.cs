﻿namespace BookSearch.Model
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
    }

    public class Login
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
    public class Register
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
