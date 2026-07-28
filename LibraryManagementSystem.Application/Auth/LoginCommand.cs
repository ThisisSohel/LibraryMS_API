using MediatR;

namespace LibraryManagementSystem.Application.Auth;

public record LoginCommand(string Username, string Password) : IRequest<LoginResultDto>;
