using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMvc.Infrastructure
{
    public static class ApiPaths
    {
        public static class Catalog
        {
            /// <summary>
            /// 商品清單uri
            /// </summary>
            /// <param name="uri"></param>
            /// <param name="type"></param>
            /// <param name="pageSize"></param>
            /// <param name="pageIndex"></param>
            /// <returns></returns>
            public static string GetAllCatalogItemsUri(string uri, int? type, int pageSize, int pageIndex)
            {
                string typeQueryString = "";
                if (type.HasValue) typeQueryString = type.Value.ToString();

                return $"{uri}items?catalogTypeId={typeQueryString}&pageIndex={pageIndex}&pageSize={pageSize}";
            }

            /// <summary>
            /// 商品類別uri
            /// </summary>
            /// <param name="baseUri"></param>
            /// <returns></returns>
            public static string GetAllTypesUri(string baseUri)
            {
                return $"{baseUri}catalogTypes";
            }
        }
    }
}
