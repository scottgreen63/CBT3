namespace CBT3_Domain.Entities;

public sealed class Trainee : BaseAuditableEntity, ITrainee
{
    public Trainee(TraineeID id) : base(id) { }
    public FirstName FirstName { get; set; }
    public LastName LastName { get; set; }
    public UPID UPID { get; set; }
    public YearOfBirth YearOfBirth { get; set; }
    public override string ToString()
    {
        return $"{FirstName.Value} {LastName.Value} {UPID.Value} {YearOfBirth.Value}";
    }
}
public class TraineeID : BaseId<string>
{
    public TraineeID(string id) : base(id) { }
}