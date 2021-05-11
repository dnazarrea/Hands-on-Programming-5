using System.Web;
using System.Web.Mvc;

namespace Hands_on_Programming_5
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
