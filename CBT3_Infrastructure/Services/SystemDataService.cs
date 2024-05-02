
namespace CBT3_Infrastructure.Services
{
    public class SystemDataService : BaseDataService<SystemDataService>
    {
        private readonly ILogger<SystemDataService> _logger;
        private readonly string _logheader;
        private SystemRepository _repo;

        public SystemDataService(ILogger<SystemDataService> logger, UserDetails userDetails, IConfiguration configuration, SystemRepository repo) : base(logger, userDetails, configuration)
        {
            _logger = logger;
            _logheader = LogSupport.LogEntryHeader(userDetails);
            //_logger.LogInformation(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.ApplicationEvent, $"{_logheader} {repo.GetType().Name} ");
            _repo = repo;
        }

      
        public Task<Result<bool>> AddAuditLogEntryAsync(AuditLogEntry auditlogentry, CancellationToken ct = default)
        {
            return _repo.AddAuditLogEntryAsync(auditlogentry,ct);
        }

        public Task<Result<int>> GetAdminPasscode()
        {
            return _repo.GetAdminPasscode();
        }


    }
}
