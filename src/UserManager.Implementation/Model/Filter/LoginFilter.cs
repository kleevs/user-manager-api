using UserManager.Model;

namespace UserManager.Implementation.Model
{
    public class LoginFilter : ILoginFilter
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
