using LibraryManagementSystem.Application.Books;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BooksController : ControllerBase
{
    private readonly ISender _sender;

    public BooksController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetAllBooksQuery(search, pageNumber, pageSize), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetBookByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateBookCommand command, CancellationToken cancellationToken)
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
        await _sender.Send(new DeleteBookCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpGet("{bookId:int}/copies")]
    public async Task<IActionResult> GetCopies(int bookId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetBookCopiesQuery(bookId), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{bookId:int}/copies")]
    public async Task<IActionResult> AddCopies(int bookId, AddBookCopiesRequest request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new AddBookCopiesCommand(bookId, request.BranchId, request.Quantity), cancellationToken);
        return Ok(result);
    }
}

public record AddBookCopiesRequest(int BranchId, int Quantity);
