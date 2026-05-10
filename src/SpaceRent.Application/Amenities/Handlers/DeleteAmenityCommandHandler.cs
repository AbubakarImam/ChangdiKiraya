using MediatR;
using SpaceRent.Application.Amenities.Commands;
using SpaceRent.Application.Interfaces;
using SpaceRent.Domain.Exceptions;

namespace SpaceRent.Application.Amenities.Handlers;

public class DeleteAmenityCommandHandler : IRequestHandler<DeleteAmenityCommand, bool>
{
    private readonly IAmenityRepository _amenityRepository;

    public DeleteAmenityCommandHandler(IAmenityRepository amenityRepository)
    {
        _amenityRepository = amenityRepository;
    }

    public async Task<bool> Handle(DeleteAmenityCommand request, CancellationToken cancellationToken)
    {
        var amenity = await _amenityRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Amenity with id '{request.Id}' was not found.");

        await _amenityRepository.DeleteAsync(amenity, cancellationToken);
        return true;
    }
}
