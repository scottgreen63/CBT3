
using Microsoft.FeatureManagement;

namespace CBT3_Application.Services;

public sealed class SystemService : IFeatureManager
{
    private readonly IFeatureManager _featureManager;
    private readonly SystemDataService _dataService;
    private readonly FileService _fileService;

    public SystemService(IFeatureManager featureManager, SystemDataService dataService,FileService fileservice)
    {
        _featureManager = featureManager;
        _dataService = dataService;
        _fileService = fileservice;
    }

    public async Task<Result<bool>> AddAuditLogEntryAsync(AuditLogEntry auditlogentry, CancellationToken ct = default)
    {
        AuditLogEntryID logentryId = new(Guid.NewGuid().ToString());
        AuditLogEntry logentry = new(logentryId);
        logentry.UserID = "SYSTEM";
        logentry.Workstation = "localhost";
        logentry.Module = "Test";
        logentry.MessageType = "Pipeline";
        logentry.Description = "Pipeline Audit Entry";
        logentry.EventDateTime = DateTime.Now.ToString("MM_dd_yyyy_hh:mm:ss");
        logentry.Function = "HandleAsync";
        logentry.Severity = "CBT3_ApplicationEventIds.Information";

        var result = await _dataService.AddAuditLogEntryAsync(logentry,ct).ConfigureAwait(false);
        if (result.IsSuccess)
            return Result<bool>.Success(true);
        else
            return Result<bool>.Failure<bool>(DomainErrors.SystemError.AuditLogEntryError);


    }

    public IAsyncEnumerable<string> GetFeatureNamesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsEnabledAsync(string feature)
    {
        return _featureManager.IsEnabledAsync(feature);
    }

    public Task<bool> IsEnabledAsync<TContext>(string feature, TContext context)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<bool>> IsFileExists(string filePath)
    {
        try
        {
            return await _fileService.IsFileExists(filePath);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure<bool>(DomainErrors.SystemError.FileExistsError);
        }

        
    }

    public Task<Result<int>> GetAdminPasscode()
    {
       return _dataService.GetAdminPasscode();
    }
}
