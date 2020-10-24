using System.Linq;
using System.Web.Mvc;

namespace HttpsRedirect.Filters
{
    public sealed class HstsHeaderAdditionFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Response.Headers.AllKeys.Contains("Strict-Transport-Security"))
                filterContext.HttpContext.Response.AddHeader("Strict-Transport-Security", "max-age=1440");
            base.OnActionExecuting(filterContext);
        }
    }
}