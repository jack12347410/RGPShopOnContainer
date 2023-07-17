using CatalogApi.Data;
using CatalogApi.Domain;
using CatalogApi.Dtos;
using CatalogApi.Services;
using CatalogApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogService _catalogService;

        public CatalogController(CatalogService service)
        {
            _catalogService = service;
        }


        /// <summary>
        /// 取得商品類型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("catalogTypes")]
        public async Task<IActionResult> GetCalalogTypes()
        {
            var result = await _catalogService.GetCatalogTypesAsync();

            return Ok(result);
        }

        /// <summary>
        /// 取得商品詳細資訊
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("items/{id}")]
        public async Task<IActionResult> GetItemVMById(int id)
        {
            var item = await _catalogService.GetItemVMByIdAsync(id);

            if (item != null)
            {
                return Ok(item);
            }

            return NotFound();
        }


        /// <summary>
        /// 取得分頁商品列表
        /// </summary>
        /// <param name="catalogTypeId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("items")]
        public async Task<IActionResult> GetItems(int? catalogTypeId,
            [FromQuery] int pageSize = 6, [FromQuery] int pageIndex = 0)
        {
            var result = await _catalogService.GetPaginatedItemsByCatalogTypeIdAsync(catalogTypeId, pageSize, pageIndex);

            return Ok(result);
        }

        /// <summary>
        /// 新增商品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("items")]
        public async Task<IActionResult> CreateProduct([FromBody] CatalogItemDto product)
        {
            var insert = await _catalogService.InsertCatalogItemAsync(product);

            return CreatedAtAction(nameof(GetItemVMById), new { id = insert.Id }, insert);
        }


        /// <summary>
        /// 更新商品
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("items")]
        public async Task<IActionResult> UpdateProductByPut([FromBody] CatalogItem product)
        {
            var update = await _catalogService.UpdateCatalogItemAsync(product);

            return update == null ? NotFound() : CreatedAtAction(nameof(GetItemVMById), new { id = update.Id }, update);
        }

        /// <summary>
        /// 刪除商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var delete = await _catalogService.DeleteCatalogItemAsync(id);

            return delete == null ? NotFound() : NoContent();
        }
    }
}
