using MediatR;
using SpaceRent.Application.Interfaces;
using SpaceRent.Application.Reviews.DTOs;

namespace SpaceRent.Application.Users.Queries.GetUserReviews;

public class GetUserReviewsQueryHandler : IRequestHandler<GetUserReviewsQuery, List<ReviewDto>>
{
    private readonly IReviewRepository _reviewRepository;

    public GetUserReviewsQueryHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<List<ReviewDto>> Handle(GetUserReviewsQuery request, CancellationToken cancellationToken)
    {
        var reviews = await _reviewRepository.GetReviewsBySpaceOwnerIdAsync(request.UserId, cancellationToken);

        return reviews.Select(r => new ReviewDto(
            r.Id,
            r.SpaceId,
            r.UserId,
            r.Rating,
            r.Comment,
            r.CreatedAt,
            r.User?.Name,
            r.User?.ProfilePictureUrl
        )).ToList();
    }
}
