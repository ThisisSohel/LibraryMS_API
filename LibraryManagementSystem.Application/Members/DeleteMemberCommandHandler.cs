using LibraryManagementSystem.Application.Common.Exceptions;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Members;

public class DeleteMemberCommandHandler : IRequestHandler<DeleteMemberCommand>
{
    private readonly IMemberRepository _memberRepository;
    private readonly ILogger<DeleteMemberCommandHandler> _logger;

    public DeleteMemberCommandHandler(IMemberRepository memberRepository, ILogger<DeleteMemberCommandHandler> logger)
    {
        _memberRepository = memberRepository;
        _logger = logger;
    }

    public async Task Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.Id);

        if (await _memberRepository.HasDependentsAsync(request.Id, cancellationToken))
        {
            throw new ConflictException("Cannot delete a member that has borrow records or reservations. Deactivate the member instead.");
        }

        _memberRepository.Delete(member);
        await _memberRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Member {MemberId} deleted: {Email}", member.Id, member.Email);
    }
}
