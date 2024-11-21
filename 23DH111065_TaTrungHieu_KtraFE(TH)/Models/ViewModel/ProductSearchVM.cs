using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList.Mvc;
namespace _23DH111065_TaTrungHieu_KtraFE_TH_.Models.ViewModel
{
    public class ProductSearchVM
    {
        
        public string SearchTerm { get; set; }
        //Search Theo Giá
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        //Thứ tự sắp xếp
        public string SortOrder { get; set; }
        //public List<Product> Products { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }   = 10;
        public PagedList.IPagedList<Product> Products { get; set; }
    }
}