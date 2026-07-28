using MediatR;

namespace LibraryManagementSystem.Application.Members;

public record GetMemberByIdQuery(int Id) : IRequest<MemberDto>;
