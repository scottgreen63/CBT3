
using CBT3_Application.Interfaces;

using static System.Formats.Asn1.AsnWriter;

namespace CBT3_Application.Services;

public sealed class LessonQuizService 
{
    private int _consecutiveFailuresInPool;
    private CourseID _courseId;
    public QuestionPoolID CurrentPoolId;
    private int _currentPoolIndex;
    public Question CurrentQuestion;
    private int _currentQuestionIndex;
    private readonly IMessenger _messenger;
    private LessonQuiz _lessonQuiz;
    private readonly IMediator _mediator;
    private List<QuestionPool> _questionPools = new();
    private Random _random;
    private Trainee _trainee;
    
    public bool IsQuizComplete() => _currentPoolIndex >= _questionPools.Count;
    public bool IsQuizFail() => _isQuizFail;
    private bool _isQuizFail { get; set; }
    public int Score { get; set; }
    

    private HashSet<QuestionID> _usedQuestionIds = new HashSet<QuestionID>();

    public LessonQuizService(IMediator mediator, IMessenger messenger)
    {
        _mediator = mediator;// CBT3_Application.Configuration.DependencyInjection.ServiceProvider.GetRequiredService<IMediator>(); ;
        _messenger = messenger;// CBT3_Application.Configuration.DependencyInjection.ServiceProvider.GetRequiredService<IMessenger>(); ;
        

    }
    public void InitializeQuiz(Trainee trainee, CourseID courseid, LessonQuiz lessonQuiz)
    {
        this._lessonQuiz = lessonQuiz;
        this._trainee = trainee;
        this._courseId = courseid;
        this._questionPools = lessonQuiz.QuestionPools;
        this._random = new Random();
        this._currentPoolIndex = -1;
        this._currentQuestionIndex = 0;
        this._consecutiveFailuresInPool = 0;
        Score = 0;
       var startlessonquizcommand = new StartLessonQuizCommand() { TraineeId = (TraineeID)trainee.Id, CourseId = courseid, LessonQuizId = (LessonQuizID)lessonQuiz.Id };
        _ = _mediator.SendAsync<Result<bool>>(startlessonquizcommand, default);

    }

    private void HandleQuizFailure()
    {
        _consecutiveFailuresInPool++;
        if (_consecutiveFailuresInPool >= _lessonQuiz.AttemptsAllowed)
        {
            _consecutiveFailuresInPool = 0;
            _isQuizFail = true;
        }
    }
    private void HandleQuizFailure(List<Answer> selectedAnswers)
    {
        _consecutiveFailuresInPool++;
        if (_consecutiveFailuresInPool >= _lessonQuiz.AttemptsAllowed)
        {
            _consecutiveFailuresInPool = 0;
            _isQuizFail = true;
            foreach (Answer answer in selectedAnswers)
            {
                AddTrainingLogEntry(answer);
            }
        }
    }
    private void HandleCorrectAnswer(Answer selectedAnswer)
    {
        _consecutiveFailuresInPool = 0;
        AddTrainingLogEntry(selectedAnswer);
    }
    private void HandleCorrectAnswers(List<Answer> selectedAnswers)
    {
        _consecutiveFailuresInPool = 0;
        foreach (Answer answer in selectedAnswers)
        {
            if (answer.IsCorrect == true)
            {
                AddTrainingLogEntry(answer);
            }
        }
    }
    private void HandleIncorrectAnswer(Answer selectedAnswer)
    {
        _consecutiveFailuresInPool++;
        if (_consecutiveFailuresInPool >= _lessonQuiz.AttemptsAllowed)
        {
            _consecutiveFailuresInPool = 0;
            _isQuizFail = true;
        }
        else
        {
            _isQuizFail = false;
        }
        AddTrainingLogEntry(selectedAnswer);
    }
    private void HandleIncorrectAnswers(List<Answer> selectedAnswers)
    {
        _consecutiveFailuresInPool++;
        if (_consecutiveFailuresInPool >= _lessonQuiz.AttemptsAllowed)
        {
            _consecutiveFailuresInPool = 0;
            _isQuizFail = true;
        }
        else
        {
            _isQuizFail = false;
        }

        foreach (Answer answer in selectedAnswers)
        {
            if (answer.IsCorrect != true)
            {
                AddTrainingLogEntry(answer);
            }
        }
    }
    private void AddTrainingLogEntry(Answer selectedAnswer)
    {
        TrainingLogEntry logentry = GetTrainingLogEntry();
        logentry.AnswerId = (AnswerID)selectedAnswer.Id;
        logentry.IsCorrect = selectedAnswer.IsCorrect ?? false;
        AddTrainingLogEntry(logentry);
    }

    private void FinishQuiz()
    {
        _messenger.Publish(new LessonQuizFinishedEvent(DateTime.Now, _lessonQuiz, QuizState.QuizFinished, string.Empty, _isQuizFail));
    }
    private bool CheckAnswer(Answer selectedAnswer)
    {
        if (selectedAnswer is null)
        {
            HandleQuizFailure();
            FinishQuiz();
            return false; // Indicate quiz failure
        }

        if (selectedAnswer.IsCorrect == true)
        {
            HandleCorrectAnswer(selectedAnswer);
            return true; // Indicate correct answer
        }
        else
        {
            HandleIncorrectAnswer(selectedAnswer);
            return false; // Indicate incorrect answer
        }
    }
    private bool CheckAnswers(List<Answer> selectedAnswers)
    {
        if (selectedAnswers is null || !selectedAnswers.Any())
        {
            HandleQuizFailure(selectedAnswers);
            FinishQuiz();
            return false; // Indicate quiz failure
        }

        int selectedCorrect = selectedAnswers.Count(a => a.IsCorrect == true);
        int countOfProvidedAnswers = selectedAnswers.Count;

        if (selectedCorrect == countOfProvidedAnswers)
        {
            HandleCorrectAnswers(selectedAnswers);
            return true; // Indicate correct answer
        }
        else
        {
            HandleIncorrectAnswers(selectedAnswers);
            return false; // Indicate incorrect answer
        }
    }

    private void ResetQuestionsInPool(QuestionPool pool)
    {
        // Clear the used question IDs for the pool
        foreach (var question in pool.Questions)
        {
            _usedQuestionIds.Remove((QuestionID)question.Id);
        }

    }
    public Question GetNextQuestion(bool movenext)
    {
        if (movenext)
        {
            _currentPoolIndex++;
        }
        // Check if there are no more question pools
        if (_currentPoolIndex >= _questionPools.Count)
        {
            _isQuizFail = false;
            FinishQuiz();
            return null; // No more questions, quiz is complete
        }


        // Get the current pool
        var currentPool = _questionPools[_currentPoolIndex];

        // If the current pool has no more questions, move to the next pool
        if (_currentQuestionIndex >= currentPool.Questions.Count)
        {
            //currentPoolIndex++;
            //currentQuestionIndex = 0; // Reset question index for the next pool
            if (_currentPoolIndex >= _questionPools.Count)
            {
                _isQuizFail = false;
                FinishQuiz();
                return null; // No more questions, quiz is complete
            }
            currentPool = _questionPools[_currentPoolIndex]; // Get the next pool
        }

        // Select a random question from the current pool
        var randomQuestionIndex = _random.Next(0, currentPool.Questions.Count);
        var randomQuestion = currentPool.Questions[randomQuestionIndex];
        //currentQuestionIndex++;
        CurrentPoolId = randomQuestion.QuestionPoolID;
        CurrentQuestion = randomQuestion;
        
        if (randomQuestion is not null)
        {
            return randomQuestion;
        }
        else
        {
            FinishQuiz();
            return null;
        }
        //return randomQuestion;
    }
    public Question GetNextQuestionFromSamePool(Question currentquestion)
    {
        if (_currentPoolIndex < 0 || _currentPoolIndex >= _questionPools.Count)
        {
            // Invalid current pool index
            FinishQuiz();
            return null;
        }

        // Get the current question pool
        var currentPool = _questionPools[_currentPoolIndex];
        currentPool.Questions.Remove(currentquestion);
        // Filter out questions that have not been used yet
        var unusedQuestions = currentPool.Questions.Where(q => !_usedQuestionIds.Contains(q.Id)).ToList();

        if (unusedQuestions.Count == 0)
        {
            // All questions in the pool have been used, reset them
            ResetQuestionsInPool(currentPool);
            unusedQuestions = currentPool.Questions.ToList();
        }

        // Select a random question from the unused questions
        var randomIndex = _random.Next(0, unusedQuestions.Count);
        var selectedQuestion = unusedQuestions[randomIndex];

        // Mark the selected question as used
        _usedQuestionIds.Add((QuestionID)selectedQuestion.Id);
        CurrentPoolId = selectedQuestion.QuestionPoolID;
        CurrentQuestion = selectedQuestion;
        
        if (selectedQuestion is not null)
        {
            return selectedQuestion;
        }
        else
        {
            FinishQuiz();
            return null;
        }
        
        //return selectedQuestion;
    }

    public Result<Question> SubmitAnswer(Answer selectedAnswer)
    {

        bool isCorrect = CheckAnswer(selectedAnswer);
        //if (isCorrect)
        //{
        //    Console.ForegroundColor = ConsoleColor.Green;
        //    string message = $"=> CORRECT <=";
        //    Console.WriteLine(message);
        //    Console.ResetColor();
        //}
        //else
        //{
        //    Console.ForegroundColor = ConsoleColor.Red;
        //    string message = $"=> INCORRECT <=";
        //    Console.WriteLine(message);
        //    Console.ResetColor();
        //}
        if (isCorrect)
        {
            Score++;
            CurrentQuestion = this.GetNextQuestion(isCorrect);
            if (CurrentQuestion != null)
            {
                CurrentPoolId = CurrentQuestion.QuestionPoolID;
            }
            return CurrentQuestion;
            // Notify Blazor to re-render the page
        }
        if (!isCorrect && !IsQuizFail())
        {
            CurrentQuestion = this.GetNextQuestionFromSamePool(CurrentQuestion);
            if (CurrentQuestion != null)
            {
                CurrentPoolId = CurrentQuestion.QuestionPoolID;
            }
            return CurrentQuestion;
            //StateHasChanged();// Notify Blazor to re-render the page
        }
        else
        {
            CurrentQuestion = null;
            return CurrentQuestion;
        }


        }

    public Result<Question> SubmitAnswers(List<Answer> providedAnswers)
    {
        var selectedAnswers = providedAnswers.Where(a => a.IsSelected).ToList();
        bool isCorrect = CheckAnswers(selectedAnswers);
        //if (isCorrect)
        //{
        //    Console.ForegroundColor = ConsoleColor.Green;
        //    string message = $"=> CORRECT <=";
        //    Console.WriteLine(message);
        //    Console.ResetColor();
        //}
        //else
        //{
        //    Console.ForegroundColor = ConsoleColor.Red;
        //    string message = $"=> INCORRECT <=";
        //    Console.WriteLine(message);
        //    Console.ResetColor();
        //}
        if (isCorrect)
        {
            Score++;
            CurrentQuestion = this.GetNextQuestion(isCorrect);
            if (CurrentQuestion != null)
            {
                CurrentPoolId = CurrentQuestion.QuestionPoolID;
            }
            return CurrentQuestion;
            //StateHasChanged(); // Notify Blazor to re-render the page
        }
        if (!isCorrect && !IsQuizFail())
        {
            CurrentQuestion = GetNextQuestionFromSamePool(CurrentQuestion);
            if (CurrentQuestion != null)
            {
                CurrentPoolId = CurrentQuestion.QuestionPoolID;
            }
            return CurrentQuestion;
        }
        else
        {
            CurrentQuestion = null;
            return CurrentQuestion;
        }

    }

    private TrainingLogEntry GetTrainingLogEntry()
{
    TraineeID traineeID = (TraineeID)_trainee.Id;
    TrainingLogEntryID tleID = new(Guid.NewGuid().ToString());
    TrainingLogEntry traininglogEntry = new(tleID);
    traininglogEntry.TraineeId = traineeID;
    traininglogEntry.CourseId = _courseId;
    traininglogEntry.LessonId = _lessonQuiz.LessonID;
    traininglogEntry.LessonQuizId = new(_lessonQuiz.Id.Value);
    traininglogEntry.QuestionPoolId = CurrentPoolId;
    traininglogEntry.QuestionId = new(CurrentQuestion.Id.Value);
    traininglogEntry.RecordedAt = DateTime.Now;

    return traininglogEntry;
}

    private void AddTrainingLogEntry(TrainingLogEntry traineeLessonQuizLogEntry)
    {
        CancellationToken ct = new();
        TrainingLogEntry logentry = traineeLessonQuizLogEntry;
        var addlogentrycommand = new AddTrainingLogEntryCommand() { TrainingLogEntry = logentry };
        _ = _mediator.SendAsync(addlogentrycommand, ct);
    }
}


