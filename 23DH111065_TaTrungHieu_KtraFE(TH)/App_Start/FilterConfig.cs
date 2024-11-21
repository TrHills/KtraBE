using System.Web;
using System.Web.Mvc;

namespace _23DH111065_TaTrungHieu_KtraFE_TH_
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
