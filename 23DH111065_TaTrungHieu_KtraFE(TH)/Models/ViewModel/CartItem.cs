﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _23DH111065_TaTrungHieu_KtraFE_TH_.Models.ViewModel
{
    public class CartItem
    {
       public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity {  get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductImage { get; set; }
        public decimal TotalPrice => Quantity*UnitPrice;
        
    }
}