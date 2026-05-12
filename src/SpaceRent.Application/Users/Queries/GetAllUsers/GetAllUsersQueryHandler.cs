using MediatR;
using SpaceRent.Application.Interfaces;
using SpaceRent.Application.Users.DTOs;

namespace SpaceRent.Application.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedResult<UserProfileDto>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PagedResult<UserProfileDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var (users, totalCount) = await _userRepository.GetPagedUsersAsync(request.PageNumber, request.PageSize, request.SearchTerm, cancellationToken);

        var dtos = users.Select(user => new UserProfileDto(
            user.Id,
            user.Name,
            user.Email ?? string.Empty,
            user.PhoneNumber,
            user.ProfilePictureUrl,
            user.Bio,
            user.Address,
            user.City,
            user.State,
            user.Country,
            user.CreatedAt,
            user.Role
        )).ToList();

        return new PagedResult<UserProfileDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
