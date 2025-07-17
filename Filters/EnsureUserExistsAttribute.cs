using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

public class EnsureUserExistsAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var userManager = context.HttpContext.RequestServices.GetService(typeof(UserManager<IdentityUser>)) as UserManager<IdentityUser>;
        var signInManager = context.HttpContext.RequestServices.GetService(typeof(SignInManager<IdentityUser>)) as SignInManager<IdentityUser>;

        var isAuthenticated = context.HttpContext.User.Identity?.IsAuthenticated ?? false;

        if (isAuthenticated)
        {
            var user = await userManager.GetUserAsync(context.HttpContext.User);
            if (user == null)
            {
                await signInManager.SignOutAsync();
                context.Result = new RedirectToActionResult("Index", "Home", null);
                return;
            }
        }

        await next(); // continue to action
    }
}
