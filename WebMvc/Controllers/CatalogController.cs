using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMvc.Services;
using WebMvc.ViewModels;

namespace WebMvc.Controllers
{
    public class CatalogController : Controller
    {
        private const int _itemsPageSize = 6;
        private ICatalogService _catalogService;
        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public async Task<IActionResult> Index(int? typesFilterApplied, int page = 0)
        {
            var catalog = await _catalogService.GetCatalogItems(typesFilterApplied, _itemsPageSize, page);
            var vm = new CatalogIndexViewModel()
            {
                CatalogItems = catalog.Data,
                Types = await _catalogService.GetTypes(),
                TypesFilterApplied = typesFilterApplied ?? 0,
                PaginationInfo = new PaginationInfo()
                {
                    ActualPage = page,
                    ItemsPerPage = Math.Min(catalog.Data.Count, _itemsPageSize),
                    TotalItems = catalog.Count,
                    TotalPages = (int)Math.Ceiling((decimal)catalog.Count / _itemsPageSize)
                }
            };

            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";
            ViewBag.TypesFilterApplied = typesFilterApplied;

            return View(vm);
        }
    }
}
