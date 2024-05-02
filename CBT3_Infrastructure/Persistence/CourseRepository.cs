using System.Collections.Frozen;
using System.Collections.Generic;

using CBT3_Domain.Entities;
using CBT3_Domain.Errors;

using CBT3_Shared.Configuration;

namespace CBT_Infrastructure.Repositories;

public sealed class CourseRepository : BaseRepository<CourseRepository, Course>
{
    private readonly ILogger<CourseRepository> _logger;
    private readonly string _logheader;
    private readonly string _connectionString;


    public CourseRepository(ILogger<CourseRepository> logger, UserDetails userdetails, IConfiguration configuration) : base(logger, userdetails, configuration)
    {

        _logger = logger;
        _logheader = LogSupport.LogEntryHeader(userdetails);
        _connectionString = ConnectionString;
        _logger.LogInformation(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.ApplicationEvent, "{logheader}  Course Repository ", _logheader);

    }


    public async Task<Result<List<Course>>> GetCoursesAsync(CancellationToken ct = default)
    {
        try
        {

            _logger.LogInformation(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.GetItems, "{logheader} {cn_spList} ", _logheader, StoredProcs.cn_spList);

            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new(StoredProcs.cn_spList, sql)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            List<Course> response = new();

            await sql.OpenAsync(ct).ConfigureAwait(false);

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false))
            {
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    //Course course = new();
                    var course = Mappers.MapToCourse(reader);
                    response.Add(course);

                }
            }

            await sql.CloseAsync().ConfigureAwait(false);

            if (response is not null)
            {
                return Result<Course>.Success(response);
            }
            else
            {
                return Result<List<Course>>.Failure<List<Course>>(DomainErrors.CourseError.NullOrEmpty);
            }
        }

        catch (Exception ex)
        {
            _logger.LogCritical(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.GetItemsError, "{logheader} {exMessage}", _logheader, ex.Message);
            return Result<List<Course>>.Failure<List<Course>>(DomainErrors.CourseError.NullOrEmpty);

        }

    }
    public async Task<Result<Course>> GetCourseByIdAsync(CourseID id, CancellationToken ct = default)
    {
        try
        {
            if (id is null)
            {
                return Result<Course>.Failure<Course>(DomainErrors.CourseError.NullOrEmpty);
            }

            _logger.LogInformation(LoggingEventIds.CBT3_InfrastructureEventIds.GetItem, "{logheader} {cn_spSelect}({id})", _logheader, StoredProcs.cn_spSelect, id);
            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new(StoredProcs.cn_spSelect, sql)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmCourseId, id.Value));

            Course? response = null;

            await sql.OpenAsync(ct).ConfigureAwait(false); ;
            using (SqlDataReader reader = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false))
            {
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    response = Mappers.MapToCourse(reader);
                    //response.Lessons = GetCourseLessonsAsync(id);
                    Result<List<Lesson>> result = await GetCourseLessonsAsync(id,ct).ConfigureAwait(false);
                    response.Lessons = result.Value;
                }
            }

            await sql.CloseAsync().ConfigureAwait(false);
            if (response is not null)
            {
                return Result<Course>.Success(response);
            }
            else
            {
                return Result<Course>.Failure<Course>(DomainErrors.CourseError.NullOrEmpty);
            }

        }
        catch (Exception ex)
        {
            _logger.LogCritical(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.GetItemError, "{_logheader} {exMessage}", _logheader, ex.Message);
            return Result<Course>.Failure<Course>(DomainErrors.GeneralError.UnProcessableRequest);
        }

    }

    private async Task<Result<List<Lesson>>> GetCourseLessonsAsync(CourseID courseId, CancellationToken ct = default)
    {
        try
        {
            if (courseId is null )
            {
                return Result<List<Lesson>>.Failure<List<Lesson>>(DomainErrors.LessonError.NullOrEmpty);
            }
            _logger.LogInformation(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.GetItems, "{logheader} {cn_spGetCourseLessons} ({courseId}) ", _logheader, StoredProcs.cn_spGetCourseLessons, courseId);

            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new(StoredProcs.cn_spGetCourseLessons, sql)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmCourseId, courseId.Value));
            List<Lesson> response = new List<Lesson>();

            await sql.OpenAsync(ct).ConfigureAwait(false);

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false))
            {
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    var lesson = Mappers.MapToLesson(reader);
                    Result<List<LessonPage>> lessonPages = await GetLessonPagesAsync((LessonID)lesson.Id,ct).ConfigureAwait(false);
                    Result<LessonQuiz> lessonQuiz = await GetLessonQuizAsync((LessonID)lesson.Id,ct).ConfigureAwait(false);

                    lesson.LessonPages = lessonPages.Value;

                    if (lessonQuiz.IsSuccess)
                    {
                        lesson.LessonQuiz = lessonQuiz.Value;
                    }
                    
                    response.Add(lesson);
                }
            }

            await sql.CloseAsync().ConfigureAwait(false);

            if (response is not null)
                return Result<List<Lesson>>.Success(response);
            else //this should never happen
                return Result<List<Lesson>>.Failure<List<Lesson>>(DomainErrors.LessonError.NullOrEmpty); // No rows were affected

        }
        catch (Exception ex)
        {
            _logger.LogCritical(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.GetItemsError, "{logheader} {exMessage}", _logheader, ex.Message);
            return  Result<List<Lesson>>.Failure<List<Lesson>>(DomainErrors.LessonError.NullOrEmpty);
            
        }
    }
    private async Task<Result<List<LessonPage>>> GetLessonPagesAsync(LessonID lessonId, CancellationToken ct = default)
    {
        try
        {
            if (lessonId is null)
            {
                return Result<List<LessonPage>>.Failure<List<LessonPage>>(DomainErrors.LessonError.NullOrEmpty);
            }
            _logger.LogInformation(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.GetItems, "{logheader} {cn_spGetLessonPages} ({lessonId})", _logheader, StoredProcs.cn_spGetLessonPages, lessonId);

            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new(StoredProcs.cn_spGetLessonPages, sql)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmLessonId, lessonId.Value));
            List<LessonPage> response = new List<LessonPage>();

            await sql.OpenAsync(ct).ConfigureAwait(false);

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false))
            {
                while (reader.Read())
                {
                    LessonPage lessonPage = Mappers.MapToLessonPage(reader);

                    response.Add(lessonPage);
                }
            }

            await sql.CloseAsync().ConfigureAwait(false);

            if (response is not null)
                return Result<List<LessonPage>>.Success(response);
            else //this should never happen
                return Result<List<LessonPage>>.Failure<List<LessonPage>>(DomainErrors.LessonError.NullOrEmpty);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.GetItemsError, "{logheader} {exMessage}", _logheader, ex.Message);
            return Result<List<LessonPage>>.Failure<List<LessonPage>>(DomainErrors.LessonError.NullOrEmpty);
        }
    }
    private async Task<Result<LessonQuiz>> GetLessonQuizAsync(LessonID lessonId,CancellationToken ct = default)
    {
        try
        {
            if (lessonId is null)
            {
                return Result<LessonQuiz>.Failure<LessonQuiz>(DomainErrors.LessonError.NullOrEmpty);
            }
            _logger.LogInformation(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.GetItems, "{logheader} {cn_spGetLessonQuiz} ({lessonId})", _logheader, StoredProcs.cn_spGetLessonQuiz, lessonId);

            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new(StoredProcs.cn_spGetLessonQuiz, sql)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmLessonId, lessonId.Value));
            LessonQuiz response = null;
            await sql.OpenAsync(ct).ConfigureAwait(false);

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false))
            {
                while (reader.Read())
                {
                    response = Mappers.MapToLessonQuiz(reader);
                    Result<List<QuestionPool>> questionpools = await GetLessonQuizQuestionPoolsAsync((LessonQuizID)response.Id,ct).ConfigureAwait(false);
                    response.QuestionPools = questionpools.Value;
                }
            }

            await sql.CloseAsync().ConfigureAwait(false);

            if (response is not null)
                return Result<LessonQuiz>.Success(response);
            else //this should never happen
                return Result<LessonQuiz>.Failure<LessonQuiz>(DomainErrors.LessonError.NullOrEmpty);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.GetItemsError, "{logheader} {exMessage}", _logheader, ex.Message);
            return  Result<LessonQuiz>.Failure<LessonQuiz>(DomainErrors.LessonError.NullOrEmpty);
        }
    }
    private async Task<Result<List<QuestionPool>>> GetLessonQuizQuestionPoolsAsync(LessonQuizID lessonquizId, CancellationToken ct = default)
    {
        try
        {
            if (lessonquizId is null)
            {
                return Result<List<QuestionPool>>.Failure<List<QuestionPool>>(DomainErrors.LessonError.NullOrEmpty);
            }
            _logger.LogInformation(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.GetItems, "{logheader} {StoredProcs.cn_sp_GetLessonQuizQuestionPools} ({lessonquizId}) ", _logheader, StoredProcs.cn_sp_GetLessonQuizQuestionPools, lessonquizId);

            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new(StoredProcs.cn_sp_GetLessonQuizQuestionPools, sql)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmLessonQuizId, lessonquizId.Value));
            
            List<QuestionPool> response = new List<QuestionPool>();

            await sql.OpenAsync(ct).ConfigureAwait(false);

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false))
            {
                while (reader.Read())
                {
                    var questionpool = Mappers.MapToQuestionPool(reader);
                    Result<List<Question>> questions = await GetQuestionPoolQuestionsAsync((QuestionPoolID)questionpool.Id,ct).ConfigureAwait(false);
                    questionpool.Questions = questions.Value;

                    response.Add(questionpool);
                }
            }

            await sql.CloseAsync().ConfigureAwait(false);

            if (response is not null)
                return Result<List<QuestionPool>>.Success(response);
            else //this should never happen
                return Result<List<QuestionPool>>.Failure<List<QuestionPool>>(DomainErrors.LessonError.NullOrEmpty);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.GetItemsError, "{_logheader} {exMessage}", _logheader, ex.Message);
            return  Result<List<QuestionPool>>.Failure<List<QuestionPool>>(DomainErrors.LessonError.NullOrEmpty);
        }
    }
    private async Task<Result<List<Question>>> GetQuestionPoolQuestionsAsync(QuestionPoolID questionpoolId, CancellationToken ct = default)
    {
        try
        {
            if (questionpoolId is null)
            {
                return Result<List<Question>>.Failure<List<Question>>(DomainErrors.LessonError.NullOrEmpty);
            }
            _logger.LogInformation(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.GetItems, "{_logheader} {cn_sp_GetQuestionPoolQuestions} ({questionpoolId})", _logheader, StoredProcs.cn_sp_GetQuestionPoolQuestions, questionpoolId);

            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new(StoredProcs.cn_sp_GetQuestionPoolQuestions, sql)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmQuestionPoolId, questionpoolId.Value));
            List<Question> response = new List<Question>();

            await sql.OpenAsync(ct).ConfigureAwait(false);

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false))
            {
                while (reader.Read())
                {
                    Question question = Mappers.MapToQuestion(reader);

                    Result<List<Answer>> answers = await GetQuestionAnswersAsync((QuestionID)question.Id,ct).ConfigureAwait(false);
                    question.Answers = answers.Value;

                    response.Add(question);
                }
            }

            await sql.CloseAsync().ConfigureAwait(false);

            if (response is not null)
                return Result<List<Question>>.Success(response);
            else //this should never happen
                return Result<List<Question>>.Failure<List<Question>>(DomainErrors.LessonError.NullOrEmpty);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.GetItemsError, "{logheader} {exMessage}", _logheader, ex.Message);
            return Result<List<Question>>.Failure<List<Question>>(DomainErrors.LessonError.NullOrEmpty);
        }
    }
    private async Task<Result<List<Answer>>> GetQuestionAnswersAsync(QuestionID questionId, CancellationToken ct = default)
    {
        try
        {
            if (questionId is null)
            {
                return Result<List<Answer>>.Failure<List<Answer>>(DomainErrors.LessonError.NullOrEmpty);
            }
            _logger.LogInformation(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.GetItems, "{logheader} {cn_sp_GetQuestionAnswers} ({questionId})", _logheader, StoredProcs.cn_sp_GetQuestionAnswers, questionId);

            using SqlConnection sql = new(_connectionString);
            using SqlCommand cmd = new(StoredProcs.cn_sp_GetQuestionAnswers, sql)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.Add(DataAccess.Parameter(ParameterNames.pmQuestionId, questionId.Value));
            List<Answer> response = new List<Answer>();

            await sql.OpenAsync(ct).ConfigureAwait(false);

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false))
            {
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    //Answer answer = new();
                    var answer = Mappers.MapToAnswer(reader);

                    response.Add(answer);
                }
            }

            await sql.CloseAsync().ConfigureAwait(false);

            if (response is not null)
                return Result<List<Answer>>.Success(response);
            else //this should never happen
                return Result<List<Answer>>.Failure<List<Answer>>(DomainErrors.LessonError.NullOrEmpty);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(CBT3_Shared.Common.LoggingEventIds.CBT3_InfrastructureEventIds.GetItemsError, "{_logheader} {exMessage}", _logheader, ex.Message);
            return Result<List<Answer>>.Failure<List<Answer>>(DomainErrors.LessonError.NullOrEmpty);
        }
    }

}

