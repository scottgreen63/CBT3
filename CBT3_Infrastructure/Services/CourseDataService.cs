using CBT3_Domain.Interfaces;

namespace CBT_Infrastructure.Services;



public class CourseDataService : BaseDataService<CourseDataService>
{
    private readonly ILogger<CourseDataService> _logger;
    private readonly string _logheader;
    private CourseRepository _repo;

    public CourseDataService(ILogger<CourseDataService> logger, UserDetails userDetails, IConfiguration configuration, CourseRepository repo) : base(logger, userDetails, configuration)
    {
        _logger = logger;
        _logheader = LogSupport.LogEntryHeader(userDetails);

        _logger.LogInformation(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.ApplicationEvent, $"{_logheader}  {repo.GetType().Name} ");
        _repo = repo;
    }

    
    public Task<Result<List<Course>>> GetCoursesAsync(CancellationToken ct = default)
    {
        return _repo.GetCoursesAsync(ct);
        
    }

    public Task<Result<Course>> GetCourseByIdAsync(CourseID id, CancellationToken ct = default)
    {
        return _repo.GetCourseByIdAsync(id, ct);
    }
    
    


}
