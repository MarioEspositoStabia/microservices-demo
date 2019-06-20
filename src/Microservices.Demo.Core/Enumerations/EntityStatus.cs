namespace Microservices.Demo.Core.Enumerations
{
    public enum EntityStatus : sbyte
    {
        HardDeleted = -2,
        SoftDeleted = -1,
        Passive = 0,
        Available = 1
    }
}
