using MediatR;
using SpaceRent.Application.Users.DTOs;

namespace SpaceRent.Application.Users.Queries.GetAllUsers;

public record GetAllUsersQuery(int PageNumber = 1, int PageSize = 10, string? SearchTerm = null) : IRequest<PagedResult<UserProfileDto>>;

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
