using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMvc.Models;

namespace WebMvc.ViewModels
{
    public class CatalogIndexViewModel
    {
        /// <summary>
        /// 商品列表
        /// </summary>
        public IEnumerable<CatalogItem> CatalogItems { get; set; }
        /// <summary>
        /// 商品類型列表
        /// </summary>
        public IEnumerable<SelectListItem> Types { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? TypesFilterApplied { get; set; }
        /// <summary>
        /// 分頁
        /// </summary>
        public PaginationInfo PaginationInfo { get; set; }
    }
}
