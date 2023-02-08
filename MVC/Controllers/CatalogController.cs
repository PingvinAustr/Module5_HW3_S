using Microsoft.AspNetCore.Mvc;
using MVC.Services.Interfaces;
using MVC.ViewModels.CatalogViewModels;
using MVC.ViewModels.Pagination;
using MVC.ViewModels;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVC.Controllers;

public class CatalogController : Controller
{
    private readonly ICatalogService _catalogService;
    private readonly HttpClient _httpClient;

    public CatalogController(ICatalogService catalogService, HttpClient httpClient)
    {
        _catalogService = catalogService;
        _httpClient = httpClient;
    }

    public async Task<IActionResult> Index(int? brandFilterApplied, int? typesFilterApplied, int? page, int? itemsPage)
    {
        page ??= 0;
        itemsPage ??= 6;
        var catalog = await GetCatalogItemsFromWebApi(page.Value, itemsPage.Value, brandFilterApplied, typesFilterApplied);
        if (catalog == null)
        {
            return View("Error");
        }
        var info = new PaginationInfo
        {
            ActualPage = page.Value,
            ItemsPerPage = itemsPage.Value,
            TotalItems = catalog.Count,
            TotalPages = (int)Math.Ceiling((decimal)catalog.Count / itemsPage.Value)
        };
        var model = new IndexViewModel
        {
            CatalogItems = catalog,
            Brands = await _catalogService.GetBrands(),
            Types = await _catalogService.GetTypes(),
            PaginationInfo = info
        };

        return View(model);
    }

    private async Task<List<CatalogItem>> GetCatalogItemsFromWebApi(int page, int itemsPage, int? brandId, int? typeId)
    {
        var queryString = "api/v1/CatalogBff/Items?";

        if (brandId.HasValue || typeId.HasValue)
        {
            queryString += "brandId=" + (brandId.HasValue ? brandId.Value.ToString() : "null") +
                           "&typeId=" + (typeId.HasValue ? typeId.Value.ToString() : "null") + "&";
        }

        queryString += $"page={page}&itemsPage={itemsPage}";

        var response = await _httpClient.GetAsync(queryString);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return await response.Content.ReadFromJsonAsync<List<CatalogItem>>();
        }
        else
        {
            return null;
        }
    }

    private async Task<IEnumerable<CatalogBrand>> GetBrandsFromWebApi()
    {
        var response = await _httpClient.GetAsync("api/v1/CatalogBff/GetBrands");
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<CatalogBrand>>();
        }
        else
        {
            return null;
        }
    }

    private async Task<IEnumerable<CatalogType>> GetTypesFromWebApi()
    {
        var response = await _httpClient.GetAsync("api/v1/CatalogBff/GetTypes");
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<CatalogType>>();
        }
        else
        {
            return null;
        }
    }
}
