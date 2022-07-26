using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WannaEat.Web.Models;
using WannaEat.Web.Services;

namespace WannaEat.Web.Controllers;

[ApiController]
[Route("api/v1/appliances")]
public class CookingAppliancesController : ControllerBase
{
    private readonly WannaEatDbContext _context;
    private readonly Logger<CookingAppliancesController> _logger;

    public CookingAppliancesController(WannaEatDbContext context, Logger<CookingAppliancesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ActionResult<IEnumerable<CookingAppliance>>> GetAppliancesPaged(
        [FromQuery(Name = "s")][Range(1, 100)] 
        int pageSize = 20,
        [FromQuery(Name = "n")][Range(1, int.MaxValue)]
        int pageNumber = 1)
    {
        var appliances = await _context.CookingAppliances
                                       .OrderBy(x => x.Id)
                                       .Skip(( pageNumber - 1 ) * pageSize)
                                       .Take(pageNumber)
                                       .ToListAsync();
        return Ok(appliances);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CookingAppliance?>> GetApplianceById(int id)
    {
        var appliance = await _context.CookingAppliances
                                      .SingleOrDefaultAsync(x => x.Id == id);
        return appliance is null
                   ? NotFound()
                   : Ok(appliance);
    }
}