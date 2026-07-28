using LibraryManagementSystem.Application.Members;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MembersController : ControllerBase
{
    private readonly ISender _sender;

    public MembersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] int? branchId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetAllMembersQuery(search, branchId, pageNumber, pageSize), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetMemberByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMemberCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateMemberCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("Route id and body id must match.");
        }

        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteMemberCommand(id), cancellationToken);
        return NoContent();
    }
}
