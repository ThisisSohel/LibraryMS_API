using LibraryManagementSystem.Application.Common.Exceptions;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Branches;

public class UpdateBranchCommandHandler : IRequestHandler<UpdateBranchCommand, BranchDto>
{
    private readonly IBranchRepository _branchRepository;
    private readonly ILogger<UpdateBranchCommandHandler> _logger;

    public UpdateBranchCommandHandler(IBranchRepository branchRepository, ILogger<UpdateBranchCommandHandler> logger)
    {
        _branchRepository = branchRepository;
        _logger = logger;
    }

    public async Task<BranchDto> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Branch), request.Id);

        if (await _branchRepository.NameExistsAsync(request.Name, excludeId: request.Id, cancellationToken))
        {
            throw new ConflictException($"A branch named '{request.Name}' already exists.");
        }

        branch.Name = request.Name;
        branch.Address = request.Address;
        branch.Phone = request.Phone;
        branch.Email = request.Email;
        branch.IsActive = request.IsActive;

        _branchRepository.Update(branch);
        await _branchRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Branch {BranchId} updated: {Name}", branch.Id, branch.Name);

        return BranchDto.FromEntity(branch);
    }
}
