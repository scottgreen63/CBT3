namespace CBT_Infrastructure.Services;

public class TrainingDataService : BaseDataService<TrainingDataService>
{
    private readonly ILogger<TrainingDataService> _logger;
    private readonly string _logheader;
    private TrainingRepository _repo;

    public TrainingDataService(ILogger<TrainingDataService> logger, UserDetails userDetails, IConfiguration configuration, TrainingRepository repo) : base(logger, userDetails, configuration)
    {
        _logger = logger;
        _logheader = LogSupport.LogEntryHeader(userDetails);

        _logger.LogInformation(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.ApplicationEvent, $"{_logheader}  {repo.GetType().Name} ");
        _repo = repo;
    }
    public async Task<Result<Trainee>> GetTraineeByIdAsync(TraineeID traineeid, CancellationToken ct = default)
    {
        return await _repo.GetTraineeByIdAsync(traineeid, ct).ConfigureAwait(false);
    }
    public async Task<Result<Trainee>> AddTraineeAsync(Trainee trainee, CancellationToken ct = default)
    {
        return await _repo.AddTraineeAsync(trainee,ct).ConfigureAwait(false); 
    }
    public async Task<Result<bool>> AddTrainingLogEntryAsync(TrainingLogEntry traininglogentry, CancellationToken ct = default)
    {
        return await _repo.AddTrainingLogEntryAsync(traininglogentry,ct).ConfigureAwait(false);
    }

    public async Task<Result<bool>> DeleteTrainingLogEntriesAsync(TraineeID  traineeId,CourseID courseId, LessonQuizID lessonquizId, CancellationToken ct = default)
    {
        return await _repo.DeleteTrainingLogEntriesAsync(traineeId, courseId, lessonquizId,ct).ConfigureAwait(false);
    }
    public async Task<Result<bool>> CompleteCourseAsync(TraineeID traineeId, CourseID courseId, bool coursepass, CancellationToken ct = default)
    {
        return await _repo.CompleteCourseAsync(traineeId,courseId, coursepass,ct).ConfigureAwait(false);
    }
    public async Task<Result<bool>> CheckForPreviousTrainingOnDateAsync(Trainee trainee, CourseID courseId, DateTime date, CancellationToken ct = default)
    {
        return await _repo.CheckForPreviousTrainingOnDateAsync(trainee, courseId, date, ct).ConfigureAwait(false);
    }

}
