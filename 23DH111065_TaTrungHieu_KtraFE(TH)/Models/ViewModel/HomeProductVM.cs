using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _23DH111065_TaTrungHieu_KtraFE_TH_.Models;
using PagedList.Mvc;

namespace _23DH111065_TaTrungHieu_KtraFE_TH_.Models.ViewModel
{
    public class HomeProductVM
    {
        public string SearchTerm { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;
        public List<Product>FeaturedProducts { get; set; }
        public PagedList.IPagedList<Product> NewProducts { get; set; }
    }
}