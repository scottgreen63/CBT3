namespace CBT3_Domain.Interfaces;

public interface IBaseEntity 
{
    // Common property for an entity identifier
    BaseId<string> Id { get; }
}
