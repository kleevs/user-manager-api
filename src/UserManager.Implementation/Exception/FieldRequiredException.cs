namespace UserManager.Implementation.Exception
{
    public class FieldRequiredException : BusinessException
    {
        public FieldRequiredException(int code, string name) : base(code, $"{name} est requis") { }
    }
}
