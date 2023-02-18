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
public class CatalogTypeController : ControllerBase
{
    private readonly ILogger<CatalogTypeController> _logger;
    private readonly ICatalogTypeService _typeService;

    public CatalogTypeController(ILogger<CatalogTypeController> logger, ICatalogTypeService typeService)
    {
        _logger = logger;
        _typeService = typeService;
    }

    [HttpPost]
    public async Task<IActionResult> Add(string type)
    {
        var result = await _typeService.Add(type);
        return Ok(new AddItemResponse<int?>() { Id = result });
    }

    [HttpDelete]
    public IActionResult Delete(int itemId)
    {
        var result = _typeService.Delete(itemId);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put(CatalogTypeDto item, int itemToUpdate)
    {
        var result = await _typeService.Put(item, itemToUpdate);
        return Ok();
    }
}