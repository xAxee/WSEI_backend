using AppCore.Dto;
using AppCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GatesController(IParkingGateService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllGates([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        if (page < 1 || size < 1)
        {
            return BadRequest("Parametry page i size muszą być większe od zera.");
        }

        return Ok(await service.GetAllPaged(page, size));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetGateById(Guid id)
    {
        var gate = await service.GetById(id);
        return gate is null ? NotFound() : Ok(gate);
    }

    [HttpGet("by-name/{name}")]
    public async Task<IActionResult> GetGateByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Nazwa bramki jest wymagana.");
        }

        var gate = await service.GetByName(name);
        return gate is null ? NotFound() : Ok(gate);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGate([FromBody] CreateGateDto dto)
    {
        var createdGate = await service.Create(dto);
        return CreatedAtAction(nameof(GetGateById), new { id = createdGate.Id }, createdGate);
    }

    [HttpPatch("{id:guid}/operational-status")]
    public async Task<IActionResult> ChangeOperationalStatus(Guid id, [FromQuery] bool isOperational)
    {
        var updatedGate = await service.ChangeOperationalStatus(id, isOperational);
        return updatedGate is null ? NotFound() : Ok(updatedGate);
    }
}