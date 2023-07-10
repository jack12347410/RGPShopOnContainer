using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMvc.Models;

namespace WebMvc.Services
{
    public interface ICatalogService
    {
        /// <summary>
        /// 取得商品清單列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        Task<Catalog> GetCatalogItems(int? type, int pageSize, int pageIndex);
        /// <summary>
        /// 取得商品類別
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SelectListItem>> GetTypes();
    }
}
