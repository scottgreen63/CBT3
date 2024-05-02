using CBT3_Domain.Errors;

namespace CBT_Infrastructure.Repositories;

public sealed class SystemRepository : BaseRepository<SystemRepository, Course>
{
    private readonly ILogger<SystemRepository> _logger;
    private readonly string _logheader;
    private readonly string _connectionString;
    private readonly IConfiguration _configuration;


    public SystemRepository(ILogger<SystemRepository> logger, UserDetails userdetails, IConfiguration configuration) : base(logger, userdetails, configuration)
    {
        _configuration = configuration;
        _logger = logger;
        _logheader = LogSupport.LogEntryHeader(userdetails);
        _connectionString = ConnectionString;
        //_logger.LogInformation(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.ApplicationEvent, "{logheader} System Repository ", _logheader);
    }

    public async Task<Result<bool>> AddAuditLogEntryAsync(AuditLogEntry auditLogEntry, CancellationToken ct = default)
    {

        try
        {
            _logger.LogInformation(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.PostItem, "{logheader} {cn_spAddAuditLogEntry}", _logheader, StoredProcs.cn_spAddAuditLogEntry); 

            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new(StoredProcs.cn_spAddAuditLogEntry, sql)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmUserID, auditLogEntry.UserID));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmMessageType, auditLogEntry.MessageType));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmSeverity, auditLogEntry.Severity));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmModule, auditLogEntry.Module));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmFunction, auditLogEntry.Function));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmDescription, auditLogEntry.Description));
            //cmd.Parameters.Add(DataAccess.Parameter("@ReturnVal", course.Id));


            await sql.OpenAsync(ct).ConfigureAwait(false);
            int rowsAffected = (int)await cmd.ExecuteScalarAsync(ct).ConfigureAwait(false);
            await sql.CloseAsync().ConfigureAwait(false);

            bool success = rowsAffected > 0;

            if (success)
                return Result<bool>.Success(true);
            else
                return Result<bool>.Failure<bool>(DomainErrors.SystemError.AuditLogEntryError);// No rows were affected
        }
        catch (Exception ex)
        {
            _logger.LogCritical(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.PostItemError, "{_logheader} {exMessage}", _logheader, ex.Message);
            return Result<bool>.Failure<bool>(DomainErrors.SystemError.AuditLogEntryError);
        }
    }

    public Task<Result<int>> GetAdminPasscode ()
    {
        return Task.FromResult<Result<int>>(Convert.ToInt32(_configuration["AdminPasscode"]));
    }
}

