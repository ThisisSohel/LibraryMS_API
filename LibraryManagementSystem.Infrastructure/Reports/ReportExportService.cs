using ClosedXML.Excel;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Application.Reports;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LibraryManagementSystem.Infrastructure.Reports;

public class ReportExportService : IReportExportService
{
    public byte[] ExportOverdueBooks(IReadOnlyList<OverdueBookDto> items, ReportExportFormat format) =>
        format == ReportExportFormat.Xlsx ? BuildOverdueBooksExcel(items) : BuildOverdueBooksPdf(items);

    public byte[] ExportMostBorrowedBooks(IReadOnlyList<MostBorrowedBookDto> items, ReportExportFormat format) =>
        format == ReportExportFormat.Xlsx ? BuildMostBorrowedBooksExcel(items) : BuildMostBorrowedBooksPdf(items);

    public byte[] ExportBranchInventorySummary(IReadOnlyList<BranchInventorySummaryDto> items, ReportExportFormat format) =>
        format == ReportExportFormat.Xlsx ? BuildBranchInventorySummaryExcel(items) : BuildBranchInventorySummaryPdf(items);

    private static readonly string[] OverdueBooksHeaders =
        ["Member", "Email", "Book", "Branch", "Borrow Date", "Due Date", "Days Overdue"];

    private static readonly string[] MostBorrowedBooksHeaders = ["Title", "Author", "Borrow Count"];

    private static readonly string[] BranchInventorySummaryHeaders =
        ["Branch", "Distinct Titles", "Total Copies", "Available Copies"];

    private static byte[] BuildOverdueBooksExcel(IReadOnlyList<OverdueBookDto> items)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Overdue Books");
        WriteHeaderRow(worksheet, OverdueBooksHeaders);

        var row = 2;
        foreach (var item in items)
        {
            worksheet.Cell(row, 1).Value = item.MemberName;
            worksheet.Cell(row, 2).Value = item.MemberEmail;
            worksheet.Cell(row, 3).Value = item.BookTitle;
            worksheet.Cell(row, 4).Value = item.BranchName;
            worksheet.Cell(row, 5).Value = item.BorrowDate;
            worksheet.Cell(row, 6).Value = item.DueDate;
            worksheet.Cell(row, 7).Value = item.DaysOverdue;
            row++;
        }

        worksheet.Columns().AdjustToContents();
        return SaveToBytes(workbook);
    }

    private static byte[] BuildOverdueBooksPdf(IReadOnlyList<OverdueBookDto> items) =>
        BuildPdf(
            "Overdue Books Report",
            OverdueBooksHeaders,
            items.Select(item => new[]
            {
                item.MemberName,
                item.MemberEmail,
                item.BookTitle,
                item.BranchName,
                item.BorrowDate.ToString("yyyy-MM-dd"),
                item.DueDate.ToString("yyyy-MM-dd"),
                item.DaysOverdue.ToString()
            }));

    private static byte[] BuildMostBorrowedBooksExcel(IReadOnlyList<MostBorrowedBookDto> items)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Most Borrowed Books");
        WriteHeaderRow(worksheet, MostBorrowedBooksHeaders);

        var row = 2;
        foreach (var item in items)
        {
            worksheet.Cell(row, 1).Value = item.Title;
            worksheet.Cell(row, 2).Value = item.Author;
            worksheet.Cell(row, 3).Value = item.BorrowCount;
            row++;
        }

        worksheet.Columns().AdjustToContents();
        return SaveToBytes(workbook);
    }

    private static byte[] BuildMostBorrowedBooksPdf(IReadOnlyList<MostBorrowedBookDto> items) =>
        BuildPdf(
            "Most Borrowed Books Report",
            MostBorrowedBooksHeaders,
            items.Select(item => new[] { item.Title, item.Author, item.BorrowCount.ToString() }));

    private static byte[] BuildBranchInventorySummaryExcel(IReadOnlyList<BranchInventorySummaryDto> items)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Branch Inventory Summary");
        WriteHeaderRow(worksheet, BranchInventorySummaryHeaders);

        var row = 2;
        foreach (var item in items)
        {
            worksheet.Cell(row, 1).Value = item.BranchName;
            worksheet.Cell(row, 2).Value = item.DistinctTitles;
            worksheet.Cell(row, 3).Value = item.TotalCopies;
            worksheet.Cell(row, 4).Value = item.AvailableCopies;
            row++;
        }

        worksheet.Columns().AdjustToContents();
        return SaveToBytes(workbook);
    }

    private static byte[] BuildBranchInventorySummaryPdf(IReadOnlyList<BranchInventorySummaryDto> items) =>
        BuildPdf(
            "Branch Inventory Summary Report",
            BranchInventorySummaryHeaders,
            items.Select(item => new[]
            {
                item.BranchName,
                item.DistinctTitles.ToString(),
                item.TotalCopies.ToString(),
                item.AvailableCopies.ToString()
            }));

    private static void WriteHeaderRow(IXLWorksheet worksheet, string[] headers)
    {
        for (var i = 0; i < headers.Length; i++)
        {
            var cell = worksheet.Cell(1, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
        }
    }

    private static byte[] SaveToBytes(XLWorkbook workbook)
    {
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    // Shared table layout for all three reports — each caller just maps its DTO rows to
    // strings first, so this has no per-report knowledge and needs no reflection.
    private static byte[] BuildPdf(string title, string[] headers, IEnumerable<string[]> rows)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4.Landscape());
                page.DefaultTextStyle(x => x.FontSize(9));

                page.Header().Column(column =>
                {
                    column.Item().Text(title).Bold().FontSize(16);
                    column.Item().Text($"Generated {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC").FontSize(8);
                });

                page.Content().PaddingTop(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        foreach (var _ in headers)
                        {
                            columns.RelativeColumn();
                        }
                    });

                    table.Header(header =>
                    {
                        foreach (var text in headers)
                        {
                            header.Cell().Element(HeaderCellStyle).Text(text).Bold();
                        }
                    });

                    foreach (var row in rows)
                    {
                        foreach (var value in row)
                        {
                            table.Cell().Element(DataCellStyle).Text(value);
                        }
                    }
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                });
            });
        });

        return document.GeneratePdf();
    }

    private static IContainer HeaderCellStyle(IContainer container) =>
        container.Background(Colors.Grey.Lighten2).PaddingVertical(4).PaddingHorizontal(4);

    private static IContainer DataCellStyle(IContainer container) =>
        container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(4).PaddingHorizontal(4);
}
