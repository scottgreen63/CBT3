
namespace CBT3_Application.Messaging.Commands;

public class CourseCompletionCommand : BaseCommandBundle, IRequest<Trainee>, IRequest<Result<bool>>
{
    public CourseCompletionCommand(TraineeID traineeId, CourseID courseId, bool coursepass)
    {
        TraineeID = traineeId;
        CourseID = courseId;
        CoursePass = coursepass;
    }
    public TraineeID TraineeID { get; init; }
    public CourseID CourseID { get; init; }
    public bool CoursePass { get; init; }

}