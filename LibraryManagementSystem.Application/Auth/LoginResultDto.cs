namespace LibraryManagementSystem.Application.Auth;

public record LoginResultDto(string Token, DateTime ExpiresAt, UserDto User);
