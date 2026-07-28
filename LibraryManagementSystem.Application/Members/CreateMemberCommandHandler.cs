using LibraryManagementSystem.Application.Common.Exceptions;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Members;

public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, MemberDto>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly ILogger<CreateMemberCommandHandler> _logger;

    public CreateMemberCommandHandler(
        IMemberRepository memberRepository,
        IBranchRepository branchRepository,
        ILogger<CreateMemberCommandHandler> logger)
    {
        _memberRepository = memberRepository;
        _branchRepository = branchRepository;
        _logger = logger;
    }

    public async Task<MemberDto> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken)
            ?? throw new NotFoundException(nameof(Branch), request.BranchId);

        if (await _memberRepository.EmailExistsAsync(request.Email, excludeId: null, cancellationToken))
        {
            throw new ConflictException($"A member with email '{request.Email}' already exists.");
        }

        var member = new Member
        {
            FullName = request.FullName,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            BranchId = request.BranchId,
            Branch = branch,
            IsActive = true
        };

        await _memberRepository.AddAsync(member, cancellationToken);
        await _memberRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Member {MemberId} created: {Email}", member.Id, member.Email);

        return MemberDto.FromEntity(member);
    }
}
