using MediatR;
using Microsoft.AspNetCore.Identity;
using SpaceRent.Domain.Entities;

namespace SpaceRent.Application.Auth.Queries.GetMe;

public class GetMeQueryHandler : IRequestHandler<GetMeQuery, User?>
{
    private readonly UserManager<User> _userManager;

    public GetMeQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<User?> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        return await _userManager.FindByIdAsync(request.UserId.ToString());
    }
}
