using CatalogApi.Dtos;
using CatalogApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogApi.Domain
{
    public class CatalogItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureFileName { get; set; }
        public int CatalogTypeId { get; set; }
        public CatalogType CatalogType { get; set; }

        public CatalogItemResponseVM ConvertToVM(string catalogBaseUrl , string filePath)
        {
            return new CatalogItemResponseVM
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                Price = this.Price,
                PictureUrl = catalogBaseUrl + string.Format(filePath, this.PictureFileName),
            };
        }

        /// <summary>
        /// 更新空值
        /// </summary>
        /// <param name="copy"></param>
        public void CopyTo(CatalogItem copy)
        {
            if (string.IsNullOrWhiteSpace(Name)) this.Name = copy.Name;
            if (string.IsNullOrWhiteSpace(Description)) this.Description = copy.Description;
            if (string.IsNullOrWhiteSpace(PictureFileName)) this.PictureFileName = copy.PictureFileName;
            if (Price == 0) this.Price = copy.Price;
            if (CatalogTypeId == 0) this.CatalogTypeId = copy.CatalogTypeId;
        }
    }
}
