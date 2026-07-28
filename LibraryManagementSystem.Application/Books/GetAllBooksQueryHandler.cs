using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Application.Common.Models;
using MediatR;

namespace LibraryManagementSystem.Application.Books;

public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, PagedResult<BookDto>>
{
    private readonly IBookRepository _bookRepository;

    public GetAllBooksQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<PagedResult<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _bookRepository.GetAllAsync(
            request.Search, request.PageNumber, request.PageSize, cancellationToken);

        var dtos = items.Select(BookDto.FromEntity).ToList();

        return new PagedResult<BookDto>(dtos, totalCount, request.PageNumber, request.PageSize);
    }
}
