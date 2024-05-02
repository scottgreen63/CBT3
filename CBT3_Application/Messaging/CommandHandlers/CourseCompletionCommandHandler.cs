
namespace CBT3_Application.Messaging.CommandHandlers;

public class CourseCompletionCommandHandler : BaseCommandBundle, IRequestHandler<CourseCompletionCommand, Result<bool>>
{
    private readonly TrainingService _trainingService;
    public CourseCompletionCommandHandler(TrainingService trainingService)
    {
        _trainingService = trainingService;
    }

    

    public async Task<Result<bool>> HandleAsync(CourseCompletionCommand request, CancellationToken ct = default)
    {
        TraineeID trainee = request.TraineeID;
        CourseID course = request.CourseID;
        bool coursepass = request.CoursePass;
        Result<bool> result = await _trainingService.CompleteCourseAsync(trainee, course, coursepass,ct).ConfigureAwait(false);
        return result;
    }
}
