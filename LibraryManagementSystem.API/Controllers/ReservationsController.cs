using LibraryManagementSystem.API.Common;
using LibraryManagementSystem.Application.Reservations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReservationsController : ControllerBase
{
    private readonly ISender _sender;

    public ReservationsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? memberId,
        [FromQuery] int? bookId,
        [FromQuery] int? branchId,
        [FromQuery] string? status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetAllReservationsQuery(memberId, bookId, branchId, status, pageNumber, pageSize), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetReservationByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateReservationCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new CancelReservationCommand(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:int}/fulfill")]
    public async Task<IActionResult> Fulfill(int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new FulfillReservationCommand(id, User.GetUserId()), cancellationToken);
        return Ok(result);
    }
}
