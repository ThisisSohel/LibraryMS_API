using LibraryManagementSystem.API.Common;
using LibraryManagementSystem.Application.BorrowRecords;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BorrowRecordsController : ControllerBase
{
    private readonly ISender _sender;

    public BorrowRecordsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? memberId,
        [FromQuery] int? branchId,
        [FromQuery] string? status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetAllBorrowRecordsQuery(memberId, branchId, status, pageNumber, pageSize), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetBorrowRecordByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Borrow(BorrowBookRequest request, CancellationToken cancellationToken)
    {
        var command = new BorrowBookCommand(request.MemberId, request.BookId, request.BranchId, User.GetUserId());
        var result = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPost("{id:int}/return")]
    public async Task<IActionResult> Return(int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new ReturnBookCommand(id), cancellationToken);
        return Ok(result);
    }
}

public record BorrowBookRequest(int MemberId, int BookId, int BranchId);
