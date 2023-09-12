using CatalogApi.Domain;
using CatalogApi.Dtos;
using CatalogApi.Repository;
using CatalogApi.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogApi.Services
{
    public class CatalogService
    {
        private readonly CatalogRepository _catalogRepository;
        private readonly IOptionsSnapshot<CatalogSettings> _settings;
        private const string _picUrlTmp = "/api/picture/{0}";

        public CatalogService(CatalogRepository repository, IOptionsSnapshot<CatalogSettings> settings)
        {
            _catalogRepository = repository;
            _settings = settings;
        }


        /// <summary>
        /// 取得商品類型
        /// </summary>
        /// <returns></returns>
        public Task<CatalogType[]> GetCatalogTypesAsync()
        {
            return _catalogRepository.FindCatalogTypesAsync();
        }

        /// <summary>
        /// 取得商品詳細資訊
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CatalogItemResponseVM> GetItemVMByIdAsync(int id)
        {
            CatalogItemResponseVM result = null;
            var item = await _catalogRepository.FindItemByIdAsync(id);

            if(item != null)
            {
                result = item.ConvertToVM(_settings.Value.ExternalCatalogBaseUrl, _picUrlTmp);
            }

            return result;
        }

        public Task<CatalogItem> GetItemByIdAsync(int id)
        {
            return _catalogRepository.FindItemByIdAsync(id);
        }

        /// <summary>
        /// 取得商品清單分頁
        /// </summary>
        /// <param name="catalogTypeId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public async Task<PaginatedItemsViewModel<CatalogItemResponseVM>> GetPaginatedItemsByCatalogTypeIdAsync(int? catalogTypeId, int pageSize, int pageIndex)
        {
            var root = _catalogRepository.FindItemsByCatalogTypeId(catalogTypeId);

            var totalItems = root.LongCountAsync();

            var itemsOnPage = await root
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();


            var list = itemsOnPage.Select(x => x.ConvertToVM(_settings.Value.ExternalCatalogBaseUrl, _picUrlTmp));

            var result = new PaginatedItemsViewModel<CatalogItemResponseVM>(pageSize, pageIndex, await totalItems, list);

            return result;
        }

        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task<CatalogItem> InsertCatalogItemAsync(CatalogItemDto dto)
        {
            return _catalogRepository.InsertItemAsync(dto);
        }

        /// <summary>
        /// 更新商品
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<CatalogItem> UpdateCatalogItemAsync(CatalogItem update)
        {
            var item = await _catalogRepository.FindItemByIdAsync(update.Id);
            if(item != null)
            {
                update.CopyTo(item);
                return await _catalogRepository.UpdateItemAsync(item, update);
            }

            return null;
        }

        /// <summary>
        /// 刪除商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CatalogItem> DeleteCatalogItemAsync(int id)
        {
            var item = await _catalogRepository.FindItemByIdAsync(id);

            if(item != null)
            {
                return await _catalogRepository.DeleteItemAsync(item);
            }

            return null;
        }
    }
}
