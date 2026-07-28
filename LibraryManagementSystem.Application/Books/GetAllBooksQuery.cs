using LibraryManagementSystem.Application.Common.Models;
using MediatR;

namespace LibraryManagementSystem.Application.Books;

public record GetAllBooksQuery(string? Search, int PageNumber = 1, int PageSize = 10) : IRequest<PagedResult<BookDto>>;
