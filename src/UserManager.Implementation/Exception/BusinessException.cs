namespace UserManager.Implementation.Exception
{
    public class BusinessException : System.Exception
    {
        public int Code { get; }
        public virtual object Content => new { Code, Message };
        public BusinessException(int code) : base() { Code = code; }
        public BusinessException(int code, string message) : base(message) { Code = code; }
    }
}
