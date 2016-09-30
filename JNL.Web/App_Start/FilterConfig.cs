using System.Web;
using System.Web.Mvc;
using JNL.Web.Filters;

namespace JNL.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionFilter());
            filters.Add(new AuthorityFilter());
        }
    }
}