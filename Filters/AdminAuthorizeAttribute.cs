using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AdminAuthorizeAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        string? role = context.HttpContext.Session.GetString("Role");

        if (role != "Admin")
        {
            context.Result = new RedirectToActionResult("Login", "Auth", null);
        }
    }
}
