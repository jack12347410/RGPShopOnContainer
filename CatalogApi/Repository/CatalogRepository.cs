using CatalogApi.Data;
using CatalogApi.Domain;
using CatalogApi.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogApi.Repository
{
    public class CatalogRepository
    {
        private readonly CatalogContext _catalogContext;

        public CatalogRepository(CatalogContext context)
        {
            _catalogContext = context;
        }

        /// <summary>
        /// 取得商品類型
        /// </summary>
        /// <returns></returns>
        public Task<CatalogType[]> FindCatalogTypesAsync()
        {
            return _catalogContext.CatalogTypes.ToArrayAsync();
        }

        /// <summary>
        /// 取得商品詳細資訊
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<CatalogItem> FindItemByIdAsync(int id)
        {
            return _catalogContext.CatalogItems.SingleOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// 取得商品清單
        /// </summary>
        /// <param name="catalogTypeId"></param>
        /// <returns></returns>
        public IQueryable<CatalogItem> FindItemsByCatalogTypeId(int? catalogTypeId)
        {
            var root = _catalogContext.CatalogItems.AsQueryable();
            if (catalogTypeId.HasValue)
            {
                root = root.Where(x => x.CatalogTypeId == catalogTypeId);
            }

            return root;
        }

        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<CatalogItem> InsertItemAsync(CatalogItemDto dto)
        {
            var item = new CatalogItem();
            _catalogContext.CatalogItems.Add(item).CurrentValues.SetValues(dto);
            await _catalogContext.SaveChangesAsync();

            return item;
        }

        /// <summary>
        /// 更新商品
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<CatalogItem> UpdateItemAsync(CatalogItem item, CatalogItem update)
        {
            _catalogContext.CatalogItems.Update(item).CurrentValues.SetValues(update);
            await _catalogContext.SaveChangesAsync();

            return item;
        }

        /// <summary>
        /// 刪除商品
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<CatalogItem> DeleteItemAsync(CatalogItem item)
        {
            _catalogContext.CatalogItems.Remove(item);
            await _catalogContext.SaveChangesAsync();

            return item;
        }
    }
}
