namespace CBT3_Domain.Entities;

public sealed class QuestionPool : BaseEntity
{
    public QuestionPool(QuestionPoolID id) : base(id) { }
    public string JumpBackStart { get; set; }
    public string JumpBackEnd { get; set; }
    public string JumpBackFilename { get; set; }
    public List<Question> Questions { get; set; } = new();
}
public class QuestionPoolID : BaseId<string>
{
    public QuestionPoolID(string id) : base(id) { }
}