
namespace CBT3_Domain.Entities;

public class TrainingLogEntry:BaseAuditableEntity
{
    public TrainingLogEntry(TrainingLogEntryID id) : base(id) { }
    public TraineeID TraineeId { get; set; }
    public CourseID CourseId { get; set; }
    public LessonID LessonId { get; set; }
    public LessonQuizID LessonQuizId { get; set; }
    public QuestionPoolID QuestionPoolId { get; set; }
    public QuestionID QuestionId { get; set; }
    public AnswerID AnswerId { get; set; }
    public bool IsCorrect { get; set; }
    public DateTime RecordedAt { get; set; }
}
public class TrainingLogEntryID : BaseId<string>
{
    public TrainingLogEntryID(string id) : base(id) { }
}