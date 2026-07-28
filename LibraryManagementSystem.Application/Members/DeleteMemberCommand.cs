using MediatR;

namespace LibraryManagementSystem.Application.Members;

public record DeleteMemberCommand(int Id) : IRequest;
