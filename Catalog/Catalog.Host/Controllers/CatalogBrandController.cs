using System.Net;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBrandController : ControllerBase
{
    private readonly ILogger<CatalogBrandController> _logger;
    private readonly ICatalogBrandService _brandService;

    public CatalogBrandController(ILogger<CatalogBrandController> logger, ICatalogBrandService brandService)
    {
        _logger = logger;
        _brandService = brandService;
    }

    [HttpPost]
    public async Task<IActionResult> Add(string brand)
    {
        var result = await _brandService.Add(brand);
        return Ok(new AddItemResponse<int?>() { Id = result });
    }

    [HttpDelete]
    public IActionResult Delete(int itemId)
    {
        var result = _brandService.Delete(itemId);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put(CatalogBrandDto item, int itemToUpdate)
    {
        var result = await _brandService.Put(item, itemToUpdate);
        return Ok();
    }
}