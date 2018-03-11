using System;
using Atut.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Atut.Filters
{
    public class RouteHelperUpdateFilter : Attribute, IActionFilter
    {
        private readonly RouteHelper _routeHelper;

        public RouteHelperUpdateFilter(RouteHelper routeHelper)
        {
            _routeHelper = routeHelper;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _routeHelper.Update((string)context.RouteData.Values["Action"], (string)context.RouteData.Values["Controller"]);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
