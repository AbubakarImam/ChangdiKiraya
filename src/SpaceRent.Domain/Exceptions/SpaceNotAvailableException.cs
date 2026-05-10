namespace SpaceRent.Domain.Exceptions;

public class SpaceNotAvailableException : DomainException
{
    public SpaceNotAvailableException(Guid spaceId) 
        : base($"The space with ID {spaceId} is not available for the requested time period.")
    {
    }
}
