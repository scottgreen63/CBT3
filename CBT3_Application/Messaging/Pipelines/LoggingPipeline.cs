using CBT3_Application.Interfaces;

using CBT3_Domain.Common;

using CBT3_Shared.Common;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace CBT3_Application.Messaging.Pipelines;


public class LoggingPipeline<TRequest, TResult> : IRequestPipelineBehavior<TRequest, TResult> where TRequest : IRequest<TResult> where TResult : Result
{
    private readonly string _logheader;
    private readonly ILogger<LoggingPipeline<TRequest, TResult>> _logger;

    public LoggingPipeline(ILogger<LoggingPipeline<TRequest, TResult>> logger)
    {
        _logger = logger;
        _logheader = LogSupport.LogPipelineEntryHeader(new());
    }
    public async Task<TResult> HandleAsync(TRequest request, RequestPipelineDelegate<TResult> next, CancellationToken cancellation = default)
    {
        var start = TimeProvider.System.GetTimestamp();
        // Log before executing the command handler
        _logger.LogInformation(CBT3_Shared.Common.LoggingEventIds.CBT3_ApplicationEventIds.Information, "{logheader} PreExecute => {TypeName}, {EventDT}", _logheader, request.GetType().Name, DateTime.Now.ToString("HH:mm:ss"));

        var result = await next();

        var diff = TimeProvider.System.GetElapsedTime(start);

        // Log after executing the command handler
        if (result.IsSuccess)
        {
            _logger.LogInformation(CBT3_Shared.Common.LoggingEventIds.CBT3_ApplicationEventIds.Information, "{logheader} PostExecute => {TypeName}, {executionTime}ms", _logheader, request.GetType().Name, diff.TotalMilliseconds);
        }
        else
        {
            _logger.LogCritical($"Logging Pipeline Validation => {result.Error.Message} {DateTime.Now.ToString("HH:mm:ss")}");
        }


        return result;
    }
}

public class AuditLogPipeline<TRequest, TResult> : IRequestPipelineBehavior<TRequest, TResult> where TRequest : IRequest<TResult> where TResult : Result
{

    private readonly SystemService _systemService;
    private readonly IFeatureManager _featureManager;

    public AuditLogPipeline(IFeatureManager featureManager, SystemService systemService)
    {
        _systemService = systemService;
        _featureManager = featureManager;

    }
    public async Task<TResult> HandleAsync(TRequest request, RequestPipelineDelegate<TResult> next, CancellationToken cancellation = default)
    {
        // Log prior to executing the command handler

        //Execute command Handler
        var result = await next();


        // Log after executing the command handler

        if (await _featureManager.IsEnabledAsync("AuditLogEnabled"))
        {

            AuditLogEntryID logentryId = new(Guid.NewGuid().ToString());
            AuditLogEntry logentry = new(logentryId);
            logentry.UserID = "SYSTEM";
            logentry.Workstation = "localhost";
            logentry.Module = request.GetType().Name;
            logentry.MessageType = "Pipeline";
            logentry.Description = "Pipeline Audit Entry";
            logentry.EventDateTime = DateTime.Now.ToString("MM_dd_yyyy_hh:mm:ss");
            logentry.Function = "HandleAsync";

            if (result.IsSuccess)
            {
                logentry.Severity = "CBT3_ApplicationEventIds.Information";
            }
            else
            {
                logentry.Severity = "CBT3_ApplicationEventIds.Critical";
            }

            await _systemService.AddAuditLogEntryAsync(logentry);
        }


        return result;
    }
}



