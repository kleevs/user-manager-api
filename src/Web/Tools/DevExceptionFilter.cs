using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UserManager.Implementation.Exception;

namespace Web.Tools
{
    public class DevExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            if (!(exception is BusinessException))
            {
                var result = new ObjectResult(exception);
                context.ExceptionHandled = true;
                context.Result = result;
                result.StatusCode = 500;
            }
        }
    }
}
