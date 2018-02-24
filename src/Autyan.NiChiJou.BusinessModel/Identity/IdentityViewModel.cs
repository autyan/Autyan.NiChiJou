﻿namespace Autyan.NiChiJou.BusinessModel.Identity
{
    public class UserRegisterModel
    {
        public string LoginName { get; set; }

        public string Password { get; set; }
    }

    public class LoginProcessModel
    {
        public string ReturnUrl { get; set; }

        public string SessionId { get; set; }
    }

    public class TokenVerificationViewMoodel
    {
        public string Token { get; set; }

        public string ReturnUrl { get; set; }
    }
}
