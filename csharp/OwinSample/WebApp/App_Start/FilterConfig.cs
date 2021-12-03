using System.Web.Mvc;

namespace OwinSample.WebApp
{
    public class FilterConfig
    {
        protected FilterConfig()
        {
            
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
