using CatalogApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogApi.Dtos
{
    public class CatalogItemDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureFileName { get; set; }
        public int CatalogTypeId { get; set; }

        public CatalogItem ConvertToItem()
        {
            return new CatalogItem
            {
                Name = this.Name,
                Description = this.Description,
                Price = this.Price,
                PictureFileName = this.PictureFileName,
                CatalogTypeId = this.CatalogTypeId,
            };
        }
    }
}
