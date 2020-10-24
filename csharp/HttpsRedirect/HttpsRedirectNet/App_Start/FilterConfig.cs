using System.Web;
using System.Web.Mvc;

namespace HttpsRedirect
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new RequireHttpsAttribute());
            filters.Add(new Filters.HstsHeaderAdditionFilter());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
