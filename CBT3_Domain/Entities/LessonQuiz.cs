namespace CBT3_Domain.Entities;

public sealed class LessonQuiz : BaseAuditableEntity
{
    public LessonQuiz(LessonQuizID id) : base(id) { }
    public LessonID LessonID { get; set; }
    public List<QuestionPool> QuestionPools { get; set; } = new();
    public int AttemptsAllowed { get; set; }
}
public class LessonQuizID : BaseId<string>
{
    public LessonQuizID(string id) : base(id) { }
}