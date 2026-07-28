using LibraryManagementSystem.Application.Common.Exceptions;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Members;

public class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberCommand, MemberDto>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly ILogger<UpdateMemberCommandHandler> _logger;

    public UpdateMemberCommandHandler(
        IMemberRepository memberRepository,
        IBranchRepository branchRepository,
        ILogger<UpdateMemberCommandHandler> logger)
    {
        _memberRepository = memberRepository;
        _branchRepository = branchRepository;
        _logger = logger;
    }

    public async Task<MemberDto> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.Id);

        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken)
            ?? throw new NotFoundException(nameof(Branch), request.BranchId);

        if (await _memberRepository.EmailExistsAsync(request.Email, excludeId: request.Id, cancellationToken))
        {
            throw new ConflictException($"A member with email '{request.Email}' already exists.");
        }

        member.FullName = request.FullName;
        member.Email = request.Email;
        member.Phone = request.Phone;
        member.Address = request.Address;
        member.BranchId = request.BranchId;
        member.Branch = branch;
        member.IsActive = request.IsActive;

        _memberRepository.Update(member);
        await _memberRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Member {MemberId} updated: {Email}", member.Id, member.Email);

        return MemberDto.FromEntity(member);
    }
}
