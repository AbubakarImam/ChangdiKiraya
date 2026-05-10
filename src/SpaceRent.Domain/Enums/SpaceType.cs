namespace SpaceRent.Domain.Enums;

[Flags]
public enum SpaceType
{
    None = 0,
    Warehouse = 1,
    EventCenter = 2,
    Workspace = 4,
    Park = 8,
    CarPark = 16,
    Other = 32
}
