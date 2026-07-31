namespace LibraryManagementSystem.Application.Reports;

public record ReportFileDto(byte[] Content, string ContentType, string FileName);
