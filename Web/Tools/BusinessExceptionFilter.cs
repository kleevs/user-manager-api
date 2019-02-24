using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UserManager.Implementation.Exception;

namespace Web.Tools
{
    public class BusinessExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception as BusinessException;
            if (exception != null)
            {
                var result = new ObjectResult(exception.Content);
                context.ExceptionHandled = true;
                context.Result = result;
                result.StatusCode = 400;
            }
        }
    }
}
