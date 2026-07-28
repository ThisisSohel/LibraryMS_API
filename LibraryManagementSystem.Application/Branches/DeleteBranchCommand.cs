using MediatR;

namespace LibraryManagementSystem.Application.Branches;

public record DeleteBranchCommand(int Id) : IRequest;
