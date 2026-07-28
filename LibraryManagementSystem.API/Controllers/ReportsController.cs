using LibraryManagementSystem.Application.Reports;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly ISender _sender;

    public ReportsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("overdue-books")]
    public async Task<IActionResult> GetOverdueBooks([FromQuery] int? branchId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetOverdueBooksReportQuery(branchId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("most-borrowed-books")]
    public async Task<IActionResult> GetMostBorrowedBooks(
        [FromQuery] int? branchId, [FromQuery] int top = 10, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetMostBorrowedBooksReportQuery(branchId, top), cancellationToken);
        return Ok(result);
    }

    [HttpGet("branch-inventory-summary")]
    public async Task<IActionResult> GetBranchInventorySummary(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetBranchInventorySummaryReportQuery(), cancellationToken);
        return Ok(result);
    }
}
