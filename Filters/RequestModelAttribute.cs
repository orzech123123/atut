using System;
using Atut.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Atut.Filters
{
    public class RequestModelAttribute : Attribute, IActionFilter
    {
        private readonly Type _type;

        public RequestModelAttribute(Type type)
        {
            _type = type;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var requestModelService = (RequestModelService)context.HttpContext.RequestServices.GetService(typeof(RequestModelService));

            var controller = context.Controller as Controller;
            var model = Activator.CreateInstance(_type);
            controller.TryUpdateModelAsync(model, _type, string.Empty).GetAwaiter().GetResult();

            requestModelService.AddModel(model);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}