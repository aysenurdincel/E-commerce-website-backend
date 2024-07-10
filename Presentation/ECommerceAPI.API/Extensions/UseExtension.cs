using Serilog.Context;

namespace ECommerceAPI.API.Extensions
{
    static public class UseExtension
    {
        public static void ConfigureUse(this WebApplication webApplication)
        {
            webApplication.Use(async (context, next) =>
            {
                var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
                LogContext.PushProperty("user_name", username);

                await next();
            });
        }
    }
}
