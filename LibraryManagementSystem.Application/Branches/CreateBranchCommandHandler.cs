using LibraryManagementSystem.Application.Common.Exceptions;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Branches;

public class CreateBranchCommandHandler : IRequestHandler<CreateBranchCommand, BranchDto>
{
    private readonly IBranchRepository _branchRepository;
    private readonly ILogger<CreateBranchCommandHandler> _logger;

    public CreateBranchCommandHandler(IBranchRepository branchRepository, ILogger<CreateBranchCommandHandler> logger)
    {
        _branchRepository = branchRepository;
        _logger = logger;
    }

    public async Task<BranchDto> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        if (await _branchRepository.NameExistsAsync(request.Name, excludeId: null, cancellationToken))
        {
            throw new ConflictException($"A branch named '{request.Name}' already exists.");
        }

        var branch = new Branch
        {
            Name = request.Name,
            Address = request.Address,
            Phone = request.Phone,
            Email = request.Email,
            IsActive = true
        };

        await _branchRepository.AddAsync(branch, cancellationToken);
        await _branchRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Branch {BranchId} created: {Name}", branch.Id, branch.Name);

        return BranchDto.FromEntity(branch);
    }
}
