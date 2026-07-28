namespace LibraryManagementSystem.Application.Reports;

public record BranchInventorySummaryDto(
    int BranchId,
    string BranchName,
    int DistinctTitles,
    int TotalCopies,
    int AvailableCopies);
