using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToliveSecond;

        public CachedAttribute(int timeToliveSecond)
        {
            _timeToliveSecond = timeToliveSecond;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
          var responsCachServicen =   context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            //Ask Clr For Createing From "ResponseCachService" Explicitly
            var cahekey = GenerateChachKeyFromRequest(context.HttpContext.Request);

            var response = await responsCachServicen.GetCachedResponseAsync(cahekey);

            if (!string.IsNullOrEmpty(response))
            {
                var result = new ContentResult()
                {
                    Content = response,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = result;
                return;
            }

           var executedActionContext = await next.Invoke(); //will Executed Action Filter Or the Action It Self

            if (executedActionContext.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null) 
            {
                await responsCachServicen.CacheResponseAsync(cahekey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToliveSecond));
            }
        }

        private string GenerateChachKeyFromRequest(HttpRequest request)
        {
            //{{url}}/api/products?pageindex=1&pageSize=5&sort=name

            var keyBuilder = new StringBuilder();


            keyBuilder.Append(request.Path); // api/product
            foreach(var (key,value ) in request.Query)
            {
                keyBuilder.Append($"|{key}-{value}");
                // api/product|pageIndex-1
                // api/product|pageIndex-1|pageSize-5
                // api/product|pageIndex-1|pageSize-5|sort-name
            }
            return keyBuilder.ToString();
        }
    }
}
