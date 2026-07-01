using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ConstructionERPSystem.Filters
{
    public class RoleAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string[] _allowedRoles;

        public RoleAuthorizeAttribute(params string[] allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role))
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            if (!_allowedRoles.Contains(role))
            {
                context.Result = new RedirectToActionResult("Unauthorized", "Auth", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}