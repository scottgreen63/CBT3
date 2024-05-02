
namespace CBT3_Application.Messaging.CommandHandlers;


    public class SubmitAnswerCommandHandler : BaseCommandBundle,IRequestHandler<SubmitAnswerCommand, Result<Question>> 
    {
    private readonly LessonQuizService _quizService;
        public SubmitAnswerCommandHandler(LessonQuizService dataService)
        {
            _quizService = dataService;
        }
       
        public async Task<Result<Question>> HandleAsync(SubmitAnswerCommand request, CancellationToken ct = default)
        {
            Result<Question> result =  _quizService.SubmitAnswer(request.Answer);
            return result;
        }

    
    }
public class SubmitAnswersCommandHandler : BaseCommandBundle, IRequestHandler<SubmitAnswersCommand, Result<Question>>
{
    private readonly LessonQuizService _quizService;
    public SubmitAnswersCommandHandler(LessonQuizService dataService)
    {
        _quizService = dataService;
    }

    public async Task<Result<Question>> HandleAsync(SubmitAnswersCommand request, CancellationToken ct = default)
    {
        Result<Question> result = _quizService.SubmitAnswers(request.Answers);
        return result;
    }


}

