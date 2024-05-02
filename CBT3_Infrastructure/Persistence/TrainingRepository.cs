using System.Diagnostics;

using CBT3_Domain.Entities;
using CBT3_Domain.Errors;
using CBT3_Domain.ValueObjects;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CBT_Infrastructure.Repositories;

public sealed class TrainingRepository : BaseRepository<TrainingRepository, Course>
{
    private readonly ILogger<TrainingRepository> _logger;
    private readonly string _logheader;
    private readonly string _connectionString;

    public TrainingRepository(ILogger<TrainingRepository> logger, UserDetails userdetails, IConfiguration configuration) : base(logger, userdetails, configuration)
    {

        _logger = logger;
        _logheader = LogSupport.LogEntryHeader(userdetails);
        _connectionString = ConnectionString;
        _logger.LogInformation(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.ApplicationEvent, "{logheader}  Training Repository ", _logheader);

    }

    public async Task<Result<Trainee>> AddTraineeAsync(Trainee trainee, CancellationToken ct = default)
    {

        try
        {
            _logger.LogInformation(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.PostItem,
                "{logheader} {cn_spAddTrainee} Id:{traineeId} First Name:{traineeFirstName} Last Name:{traineeLastName} UPID:{traineeUPID} YOB:{traineeYearOfBirth}",
                _logheader, StoredProcs.cn_spAddTrainee, trainee.Id, trainee.FirstName, trainee.LastName, trainee.UPID, trainee.YearOfBirth);

            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new(StoredProcs.cn_spAddTrainee, sql)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmTraineeId, trainee.Id.Value));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmFirstName, trainee.FirstName.Value));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmLastName, trainee.LastName.Value));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmUPID, trainee.UPID.Value));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmYearOfBirth, trainee.YearOfBirth.Value));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmCreatedBy, "SYSTEM"));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmCreatedDate, trainee.CreatedDate.Value));

            await sql.OpenAsync(ct).ConfigureAwait(false);
            var result = cmd.ExecuteScalar();//ABSOLUTELY CAN't BE Async!
            TraineeID traineeid = new(result.ToString());
            Trainee newtrainee = GetTraineeByIdAsync(traineeid, ct).Result.Value;
            await sql.CloseAsync().ConfigureAwait(false);

            if (newtrainee is not null)
                return Result<Trainee>.Success(newtrainee);
            else //this should never happen
                return Result<Trainee>.Failure<Trainee>(DomainErrors.TraineeError.NullOrEmpty); // No rows were affected

        }
        catch (Exception ex)
        {
            _logger.LogCritical(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.PostItemError, "{logheader} {exMessage}", _logheader, ex.Message);
            return Result<Trainee>.Failure<Trainee>(DomainErrors.TraineeError.NullOrEmpty); // Or provide a more specific error code
        }
    }
    
    public async Task<Result<bool>> AddTrainingLogEntryAsync(TrainingLogEntry trainingLog, CancellationToken ct = default)
    {

        try
        {
            if (trainingLog is null)
            {
                return Result<bool>.Failure<bool>(DomainErrors.TrainingLogEntryError.NullOrEmptyParam);
            }
            
            _logger.LogInformation(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.PostItem, "{logheader} {cn_spInsertTrainingLogEntry} Question {questionId} Answer {anserId} ", _logheader, StoredProcs.cn_spInsertTrainingLogEntry, trainingLog.QuestionId.Value, trainingLog.AnswerId.Value); 
                                                                                                                        
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new(StoredProcs.cn_spInsertTrainingLogEntry, sql)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmTraineeId, trainingLog.TraineeId.Value));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmCourseId, trainingLog.CourseId.Value));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmLessonId, trainingLog.LessonId.Value));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmLessonQuizId, trainingLog.LessonQuizId.Value));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmQuestionPoolId, trainingLog.QuestionPoolId.Value));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmQuestionId, trainingLog.QuestionId.Value));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmAnswerId, trainingLog.AnswerId.Value));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmIsCorrect, trainingLog.IsCorrect));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmRecordedAt, trainingLog.RecordedAt));
            cmd.Parameters.Add(DataAccess.Parameter("@ReturnVal", 0));

            await sql.OpenAsync(ct).ConfigureAwait(false);
            int rowsAffected = (int)await cmd.ExecuteScalarAsync(ct).ConfigureAwait(false);

            await sql.CloseAsync().ConfigureAwait(false);

            if (rowsAffected > 0)
                return Result<bool>.Success(true);
            else //this should never happen
                return (Result<bool>)Result<bool>.Failure(DomainErrors.TrainingLogEntryError.AddTrainingLogEntry); // No rows were affected
        }
        catch (Exception ex)
        {
            _logger.LogCritical(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.PostItemError, $"{_logheader} {ex.Message}");
            return Result<bool>.Failure<bool>(DomainErrors.TrainingLogEntryError.AddTrainingLogEntry); // Or provide a more specific error code
        }
    }
    
    public async Task<Result<bool>> DeleteTrainingLogEntriesAsync(TraineeID traineeId, CourseID courseId, LessonQuizID lessonquizId, CancellationToken ct = default)
    {

        try
        {
            if (traineeId is null || courseId is null)
            {
                return Result<bool>.Failure<bool>(DomainErrors.TrainingLogEntryError.NullOrEmptyParam);
            }
            _logger.LogInformation(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.PostItem, "{_logheader} {cn_spDeleteTrainingLogEntries} Trainee {traineeId} Course {courseId}", _logheader, StoredProcs.cn_spDeleteTrainingLogEntries, traineeId, courseId);

            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new(StoredProcs.cn_spDeleteTrainingLogEntries, sql)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmTraineeId, traineeId.Value));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmCourseId, courseId.Value));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmLessonQuizId, lessonquizId.Value));
            cmd.Parameters.Add(DataAccess.Parameter("@ReturnVal", 0));

            await sql.OpenAsync(ct).ConfigureAwait(false);
            int rowsAffected = (int)await cmd.ExecuteScalarAsync(ct).ConfigureAwait(false);

            await sql.CloseAsync().ConfigureAwait(false);

            if (rowsAffected >= 0)
                return Result<bool>.Success(true);
            else
                return Result<bool>.Failure<bool>(DomainErrors.TrainingLogEntryError.DeleteTrainingLog);// No rows were affected
        }
        catch (Exception ex)
        {
            _logger.LogCritical(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.PostItemError, $"{_logheader} {ex.Message}");
            return Result<bool>.Failure<bool>(DomainErrors.TrainingLogEntryError.DeleteTrainingLog); // Or provide a more specific error code
        }
    }
    
    public async Task<Result<bool>> CompleteCourseAsync(TraineeID traineeId, CourseID courseId, bool coursepass, CancellationToken ct = default)
    {

        try
        {
            if (traineeId is null || courseId is null)
            {
                return Result<bool>.Failure<bool>(DomainErrors.TrainingLogEntryError.NullOrEmptyParam);
            }

            _logger.LogInformation(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.PostItem, "{_logheader} {cn_spCompleteCourse} Trainee {traineeId} Course {courseId}", _logheader, StoredProcs.cn_spCompleteCourse, traineeId, courseId);
            
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new(StoredProcs.cn_spCompleteCourse, sql)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmTraineeId, traineeId.Value));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmCourseId, courseId.Value));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmCoursePass, coursepass));
            cmd.Parameters.Add(DataAccess.Parameter("@ReturnVal", 0));

            await sql.OpenAsync(ct).ConfigureAwait(false);
            int rowsAffected = (int)await cmd.ExecuteScalarAsync(ct).ConfigureAwait(false);

            await sql.CloseAsync().ConfigureAwait(false);
             

            bool success = rowsAffected > -1;

            if (success)
                return Result<bool>.Success(true);
            else
                return Result<bool>.Failure<bool>(DomainErrors.TrainingLogEntryError.CourseCompletion);// No rows were affected


        }
        catch (Exception ex)
        {

            _logger.LogCritical(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.PostItemError, "{_logheader} {exMessage}", _logheader, ex.Message);
            return Result<bool>.Failure<bool>(DomainErrors.TrainingLogEntryError.CourseCompletion);
        }
    }

    public async Task<Result<Trainee>> GetTraineeByIdAsync(TraineeID traineeId, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.GetItems, "{_logheader} {cn_spGetTrainee} ", _logheader, StoredProcs.cn_spGetTrainee);

            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new(StoredProcs.cn_spGetTrainee, sql)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmTraineeId, traineeId.Value));
            Trainee response = null;

            await sql.OpenAsync(ct).ConfigureAwait(false);

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false))
            {
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    var trainee = Mappers.MapToTrainee(reader);

                    response = trainee;
                }
            }

            await sql.CloseAsync().ConfigureAwait(false);

            if (response is not null)
                return Result<Trainee>.Success(response);
            else //this should never happen
                return (Result<Trainee>)Result<Trainee>.Failure(DomainErrors.TraineeError.NullOrEmpty); 
        }
        catch (Exception ex)
        {
            _logger.LogCritical(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.GetItemsError, "{_logheader} {exMessage}", _logheader, ex.Message);
            return (Result<Trainee>)Result<Trainee>.Failure(DomainErrors.TraineeError.NullOrEmpty);
        }
    }

    public async Task<Result<bool>> CheckForPreviousTrainingOnDateAsync(Trainee trainee, CourseID courseID, DateTime datetocheck, CancellationToken ct = default)
    {
     
        try
        {
            if (trainee is null)
            {
                return Result<bool>.Failure<bool>(DomainErrors.CourseError.CourseCheck);
            }

            _logger.LogInformation(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.PostItem, "{logheader} {cn_spCheckForPreviousTrainingOnDate} Trainee {traineeId} CourseID {courseID} DateToCheck {datetocheck}", _logheader, StoredProcs.cn_spCheckForPreviousTrainingOnDate, trainee.Id.Value, courseID.Value, datetocheck);

            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new(StoredProcs.cn_spCheckForPreviousTrainingOnDate, sql)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmFirstName, trainee.FirstName.Value));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmLastName, trainee.LastName.Value));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmUPID, trainee.UPID.Value));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmYearOfBirth, trainee.YearOfBirth.Value));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmCourseCode, courseID.Value));
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmDate, datetocheck.ToString("yyyy-MM-dd HH:mm:ss")));
            cmd.Parameters.Add(DataAccess.Parameter("@pTrainingExists", 0));

            await sql.OpenAsync(ct).ConfigureAwait(false);
            bool trainingexists = (bool)await cmd.ExecuteScalarAsync(ct).ConfigureAwait(false);

            await sql.CloseAsync().ConfigureAwait(false);

            return Result<bool>.Success(trainingexists); // No rows were affected
        }
        catch (Exception ex)
        {
            _logger.LogCritical(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.PostItemError, $"{_logheader} {ex.Message}");
            return Result<bool>.Failure<bool>(DomainErrors.CourseError.CourseCheck); // Or provide a more specific error code
        }
    }
}

