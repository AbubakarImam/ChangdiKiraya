using MediatR;
using SpaceRent.Application.Reviews.DTOs;

namespace SpaceRent.Application.Users.Queries.GetUserReviews;

public record GetUserReviewsQuery(Guid UserId) : IRequest<List<ReviewDto>>;
