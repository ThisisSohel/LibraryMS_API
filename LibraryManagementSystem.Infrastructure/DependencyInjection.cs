using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Infrastructure.Auth;
using LibraryManagementSystem.Infrastructure.Persistence;
using LibraryManagementSystem.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagementSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention());

        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IBookCopyRepository, BookCopyRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBorrowRecordRepository, BorrowRecordRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IReportsRepository, ReportsRepository>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}
