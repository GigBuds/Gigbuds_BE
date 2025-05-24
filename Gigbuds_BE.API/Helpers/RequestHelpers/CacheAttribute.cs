using Microsoft.AspNetCore.Mvc.Filters;

namespace Gigbuds_BE.API.Helpers.RequestHelpers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        /*
            Cache Hit
            Request → Generate Key → Check Cache → Found → Return Cached Data

            Cache Miss
            Request → Generate Key → Check Cache → Not Found → Execute Action → Store Data to Cache
        */

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //var cacheService = context.HttpContext.RequestServices.GetRequiredService<IRespons>
            return Task.CompletedTask;
        }
    }
}
