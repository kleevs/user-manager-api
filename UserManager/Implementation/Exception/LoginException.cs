namespace UserManager.Implementation.Exception
{
    public class LoginException : BusinessException
    {
        public LoginException() : base(Constant.CodeError.LoginFailed, "Login failed") { }
    }
}
