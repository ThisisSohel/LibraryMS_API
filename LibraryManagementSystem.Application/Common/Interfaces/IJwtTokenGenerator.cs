using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    (string Token, DateTime ExpiresAt) GenerateToken(User user);
}
