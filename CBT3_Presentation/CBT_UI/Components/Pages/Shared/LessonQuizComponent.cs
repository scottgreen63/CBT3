using CBT3_Application.Interfaces;
using CBT3_Application.Messaging;
using CBT3_Application.Services;

using CBT3_Domain.Entities;
using CBT3_Domain.Enums;
using CBT3_Domain.Events.DomainEvents;

using Microsoft.AspNetCore.Components;

using Radzen;

using static System.Formats.Asn1.AsnWriter;

namespace CBT3_UI.Components.Shared
{
    public partial class LessonQuizComponent : ComponentBase , IDisposable
    {
        [Inject]
        //CourseMachine _courseMachine { get; set; }
        IMediator _mediator { get; set; }  //=> CBT3_Application.Configuration.DependencyInjection.ServiceProvider.GetRequiredService<IMediator>();
        [Inject]
        IMessenger _messenger { get; set; } //=> CBT3_Application.Configuration.DependencyInjection.ServiceProvider.GetRequiredService<IMessenger>();

        [Inject]
        DialogService _dialogService { get; set; }
        [Inject]
        LessonQuizService _lessonQuizSvc { get; set; }
        [Parameter]
        public LessonQuiz LessonQuiz { get; set; }

        [Parameter]
        public Trainee Trainee { get; set; }

        [Parameter]
        public CourseID CourseID { get; set; }


        public Answer ParentAnswer;
        public int Score = 0;
        public Question CurrentQuestion;
        public bool IsQuizFailure;


        protected LessonQuiz _lessonquiz;
        protected Trainee _trainee;
        protected CourseID _courseID;
        protected Lesson _lesson;
        protected string _currentPoolId;

        

        

        protected override async Task OnInitializedAsync()
        {
            
            try
            {
                Score = 0;
                _lessonquiz = LessonQuiz;
                _trainee = Trainee;
                _courseID = CourseID;
                               
                if (_lessonquiz != null)
                {
                    //_lessonquiz = _lesson.LessonQuiz;
                    _lessonQuizSvc.InitializeQuiz(_trainee, _courseID,_lessonquiz);
                    //_mediator = CBT3_Application.Configuration.DependencyInjection.ServiceProvider.GetRequiredService<IMediator>();
                    //_messenger = CBT3_Application.Configuration.DependencyInjection.ServiceProvider.GetRequiredService<IMessenger>();

                    await LoadQuestion();
                    
                    CurrentQuestion = _lessonQuizSvc.CurrentQuestion;
                    _currentPoolId = CurrentQuestion.QuestionPoolID.Value; 
                }
                else
                {
                    // Handle the case where no suitable lesson is found
                    // For example, display an error message or redirect the user
                    // You can also set default values for _lessonquiz, _questionPools, _questions if needed
                }


            }
            catch (Exception ex)
            {
                // Handle exceptions
                // For example, log the exception or display an error message
            }

            await base.OnInitializedAsync();
        }

        private async Task LoadQuestion()
        {
            CurrentQuestion = _lessonQuizSvc.GetNextQuestion(true);
            _currentPoolId = CurrentQuestion.QuestionPoolID.Value;
            StateHasChanged(); // Ensure the UI updates with the new question
        }
        

        private void TFQuestionAnsweredEventEvent(TFQuestionAnsweredEvent @event)
        {
            Answer answer = @event.Answer;
            var results = _lessonQuizSvc.SubmitAnswer(answer);
            if (results.IsSuccess)
            {
                CurrentQuestion = results.Value;
            }
            else if (results is null)
            {
                IsQuizFailure = _lessonQuizSvc.IsQuizFail();
                _messenger.Publish(new LessonQuizFinishedEvent(DateTime.Now, _lessonquiz, QuizState.QuizFinished, string.Empty, IsQuizFailure));
                //ExitState(_isFail);
            }


        }
        private void MCQuestionAnsweredEventEvent(MCQuestionAnsweredEvent @event)
        {
            List<Answer> answers = @event.SubmittedAnswers;

            var results = _lessonQuizSvc.SubmitAnswers(answers);
            if (results.IsSuccess)
            {
                CurrentQuestion = results.Value;
            }
            else if (results is null)
            {
                IsQuizFailure = _lessonQuizSvc.IsQuizFail();
                _messenger.Publish(new LessonQuizFinishedEvent(DateTime.Now, _lessonquiz, QuizState.QuizFinished, string.Empty, IsQuizFailure));
                //ExitState(_isFail);
            }

        }
        public void SubmitAnswer(Answer selectedAnswer)
        {
            SubmitAnswerCommand cmd = new(selectedAnswer);

            var result = _mediator.SendAsync(cmd, default);
            ///      _lessonQuizSvc.SubmitAnswer(selectedAnswer);
            bool isCorrect = selectedAnswer.IsCorrect ?? false;
            Score = _lessonQuizSvc.Score;
            IsQuizFailure = _lessonQuizSvc.IsQuizFail();
            CurrentQuestion = result.Result.Value;
            //_currentPoolId = CurrentQuestion.QuestionPoolID.Value;
            StateHasChanged();
            //return;
            //if (isCorrect)
            //{
            //    if (CurrentQuestion != null)
            //    {
            //        _currentPoolId = CurrentQuestion.QuestionPoolID.Value;
            //    }

            //    StateHasChanged(); // Notify Blazor to re-render the page
            //}
            //if (!isCorrect && !IsQuizFailure)
            //{
            //    CurrentQuestion = _lessonQuizSvc.GetNextQuestionFromSamePool(CurrentQuestion);
            //    if (CurrentQuestion != null)
            //    {
            //        _currentPoolId = CurrentQuestion.QuestionPoolID.Value;
            //    }

            //    StateHasChanged();// Notify Blazor to re-render the page
            //    return;
            //}



        }
        public void SubmitAnswers(List<Answer> providedAnswers)
        {
            var selectedAnswers = providedAnswers.Where(a => a.IsSelected).ToList();
            //var allCorrect = selectedAnswers.All(a => a.IsCorrect ?? false);
            //var result  = _lessonQuizSvc.SubmitAnswers(selectedAnswers);
            SubmitAnswersCommand cmd = new(selectedAnswers);

            var result = _mediator.SendAsync(cmd, default);
            Score = _lessonQuizSvc.Score;
            IsQuizFailure = _lessonQuizSvc.IsQuizFail();
            CurrentQuestion = result.Result.Value;
            StateHasChanged();
            //if (allCorrect)
            //{
            //    CurrentQuestion = _lessonQuizSvc.GetNextQuestion(allCorrect);
            //    if (CurrentQuestion != null)
            //    {
            //        _currentPoolId = CurrentQuestion.QuestionPoolID.Value;
            //    }

            //    StateHasChanged(); // Notify Blazor to re-render the page
            //}
            //if (!allCorrect && !IsQuizFailure)
            //{
            //    CurrentQuestion = _lessonQuizSvc.GetNextQuestionFromSamePool(CurrentQuestion);
            //    if (CurrentQuestion != null)
            //    {
            //        _currentPoolId = CurrentQuestion.QuestionPoolID.Value;
            //    }

            //    StateHasChanged();// Notify Blazor to re-render the page
            //    return;
            //}
        }



        protected void Finish()
        {
            //IsQuizFailure = false;
            //Score = 0;
            _messenger.Publish(new LessonQuizFinishedEvent(DateTime.Now, _lessonquiz, QuizState.QuizFinished, string.Empty, IsQuizFailure));
            _dialogService.Close(true);
        }


        protected void RestartQuiz()
        {
            Score = 0;

        }


        public void ChildFiredEvent(Answer answer)
        {
            ParentAnswer = answer;
            StateHasChanged();
        }


        private bool disposedValue = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    // This is where you can release any resources like event handlers, timers, etc.
                }

                // Dispose unmanaged resources
                // This is where you can release any unmanaged resources if needed

                disposedValue = true;
            }
        }

        // Finalizer
        ~LessonQuizComponent()
        {
            Dispose(false);
        }
    }
}

