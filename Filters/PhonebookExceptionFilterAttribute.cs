using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Phonebook.Filters
{
    public class PhonebookAsyncExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        void IExceptionFilter.OnException(ExceptionContext context)
        {
            string? actionName = context.ActionDescriptor.DisplayName;
            string? exceptionStack = context.Exception.StackTrace;
            string exceptionMessage = context.Exception.Message;
            context.Result = new ContentResult
            {
                Content = $"In the method {actionName} exception happend: \n {exceptionMessage} \n {exceptionStack}"
            };
            context.ExceptionHandled = true;
        }
    }
}