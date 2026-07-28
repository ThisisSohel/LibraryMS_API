using LibraryManagementSystem.Application.Common.Exceptions;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Branches;

public class DeleteBranchCommandHandler : IRequestHandler<DeleteBranchCommand>
{
    private readonly IBranchRepository _branchRepository;
    private readonly ILogger<DeleteBranchCommandHandler> _logger;

    public DeleteBranchCommandHandler(IBranchRepository branchRepository, ILogger<DeleteBranchCommandHandler> logger)
    {
        _branchRepository = branchRepository;
        _logger = logger;
    }

    public async Task Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Branch), request.Id);

        if (await _branchRepository.HasDependentsAsync(request.Id, cancellationToken))
        {
            throw new ConflictException("Cannot delete a branch that has staff, members, book copies, or reservations assigned to it.");
        }

        _branchRepository.Delete(branch);
        await _branchRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Branch {BranchId} deleted: {Name}", branch.Id, branch.Name);
    }
}
