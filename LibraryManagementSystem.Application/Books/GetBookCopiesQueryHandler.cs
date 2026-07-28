using LibraryManagementSystem.Application.Common.Exceptions;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using MediatR;

namespace LibraryManagementSystem.Application.Books;

public class GetBookCopiesQueryHandler : IRequestHandler<GetBookCopiesQuery, List<BookCopyDto>>
{
    private readonly IBookRepository _bookRepository;
    private readonly IBookCopyRepository _bookCopyRepository;

    public GetBookCopiesQueryHandler(IBookRepository bookRepository, IBookCopyRepository bookCopyRepository)
    {
        _bookRepository = bookRepository;
        _bookCopyRepository = bookCopyRepository;
    }

    public async Task<List<BookCopyDto>> Handle(GetBookCopiesQuery request, CancellationToken cancellationToken)
    {
        _ = await _bookRepository.GetByIdAsync(request.BookId, cancellationToken)
            ?? throw new NotFoundException(nameof(Book), request.BookId);

        var copies = await _bookCopyRepository.GetByBookIdAsync(request.BookId, cancellationToken);

        return copies.Select(BookCopyDto.FromEntity).ToList();
    }
}
