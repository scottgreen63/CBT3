using Blazored.Video.Support;

using CBT_Infrastructure.Services;

using CBT_UI.Components.Pages.Shared;


using CBT3_Application.Interfaces;
using CBT3_Application.Messaging;
using CBT3_Application.Messaging.Commands;
using CBT3_Application.Messaging.Queries;
using CBT3_Application.Services;

using CBT3_Domain.Common;
using CBT3_Domain.Entities;
using CBT3_Domain.Enums;
using CBT3_Domain.Errors;
using CBT3_Domain.Events.DomainEvents;
using CBT3_Domain.Events.SystemEvents;
using CBT3_Domain.Interfaces;

using CBT3_UI;
using CBT3_UI.Components.Shared;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Radzen;



namespace CBT_UI.Components.Pages;

public partial class CoursePlayer :ComponentBase
{
    [Inject]
    protected SystemService _systemService { get; set; }
    

    void OnEnd(VideoState state)
    {
        _dialogService.Close(false); _dialogService.Close(false);

    }
    private Course _course = null;
    private Trainee _trainee = null;
    
    private MarkupString _message;
    private MarkupString _courseMessage;
    private bool? _fileExists;
    private string _videoFileName = string.Empty;
    private string _currentEvent = string.Empty;
    private MachineState _currentState;
    public string _coursestartButtonStyle = "";
    public string _messageStyle = "display:none";
    public string _courseMessageStyle = "display:none";
    public string _continueButton = "display:none";
    public string _startStyle = "";
    public string _continueStyle = "display:none";
    public string _coursecompleted = "display:none";


    [Parameter]
    public string TraineeId { get; set; }
    private string _traineeId;
    [Parameter]
    public string CourseId { get; set; }
    private string _courseId;

    
    private async Task InitializeMachine()
    {
        //"BLUE_C2120"
        //"C1200"
        TraineeID traineeId = new TraineeID(_traineeId);
        GetTraineeQuery traineequery = new GetTraineeQuery(traineeId);
        Result<Trainee> traineeresult = await _mediator.SendAsync(traineequery, default);
        if (traineeresult.IsSuccess)
        {
            _trainee = traineeresult.Value;
        }



        CourseID courseId = new(_courseId);
        GetCourseQuery courseQuery = new(courseId);
        Result<Course> courseresult = await _mediator.SendAsync(courseQuery, default);

        if (courseresult.IsSuccess)
        {
            _course = courseresult.Value;
        }

        _courseMachine.InitializeMachine(_trainee, _course);

    }

    protected override async Task OnInitializedAsync()
    {
        SubscribeToEvents();
        _courseId = CourseId;
        _traineeId = TraineeId;
        
        await base.OnInitializedAsync();
    }


    public async Task<Result<bool>> ShowVideo(string filename)
    {
        string fileName = filename;

        bool dialog = await Task.Run(() => _dialogService.OpenAsync<VideoComponent>($"Video",
            new Dictionary<string, object>() { { "FileName", fileName } },
            new DialogOptions() { Width = "1300px", Height = "700px", Resizable = false, Draggable = false, ShowClose = true, ShowTitle = false, CssClass = "radzen-dialog" }));


        if (dialog)
        {
            return Result.Success<bool>(dialog);
        }
        else
        {
            return Result.Failure<bool>(DomainErrors.SystemError.VideoPlaybackError);
        }
    }
    public async Task ShowQuiz(Trainee trainee, CourseID courseid, LessonQuiz quiz)
    {
        bool dialog = await _dialogService.OpenAsync<LessonQuizComponent>($"Quiz",
            new Dictionary<string, object>() { { "LessonQuiz", quiz }, { "Trainee", trainee }, { "CourseID", courseid } },
            new DialogOptions() { Width = "1300px", Height = "1000px", Resizable = false, Draggable = false, ShowClose = true, ShowTitle = false, CssClass = "radzen-dialog" });

        // if (IsQuizFailure)
        // {
        //     Lesson lesson = _course.Lessons.Where(q => q.LessonQuiz.Id == quiz.Id).FirstOrDefault();
        //     LessonPage page = lesson.LessonPages.Where(p => p.LessonPageType == "PT_BASE_08").FirstOrDefault();
        //     _headermessagestyle = "";
        //     headingmessage = (MarkupString)page.PageText;
        //     _continuebutton  = "display:none";
        // }
        if (dialog)
        {
            ContinueClick();
        }

    }

    private async Task CourseMachineStart()
    {
        await Task.Run(() => InitializeMachine());
        _startStyle = "display:none";

        MachineStartCommand request = new MachineStartCommand(_courseMachine);
        var result = await _mediator.SendAsync(request, default);

        StateHasChanged();
    }
    private void ContinueClick()
    {
        if (_currentState.Equals(MachineState.CourseMachinePaused))
        {
            CourseMachineResume();
            return;
        }
        if (_currentState.Equals(MachineState.LessonMachinePaused))
        {
            LessonMachineResume();
            return;
        }
        if (_currentState.Equals(MachineState.LessonPageMachinePaused))
        {
            LessonPageMachineResume();
            return;
        }


    }

    private void SubscribeToEvents()
    {
        _messenger.Subscribe<CourseStateEvent>(HandleCourseStateEvent);
        _messenger.Subscribe<LessonPageEvent>(HandleLessonPageEvent);
        _messenger.Subscribe<MachinePauseEvent>(HandleMachinePauseEvent);
        _messenger.Subscribe<LessonQuizStartedEvent>(HandleLessonQuizStartedEvent);
        _messenger.Subscribe<LessonQuizFinishedEvent>(HandleLessonQuizFinishedEvent);
    }
    private void UnsubscribeToEvents()
    {
        _messenger.Unsubscribe<CourseStateEvent>(HandleCourseStateEvent);
        _messenger.Unsubscribe<LessonPageEvent>(HandleLessonPageEvent);
        _messenger.Unsubscribe<MachinePauseEvent>(HandleMachinePauseEvent);
        _messenger.Unsubscribe<LessonQuizStartedEvent>(HandleLessonQuizStartedEvent);
        _messenger.Unsubscribe<LessonQuizFinishedEvent>(HandleLessonQuizFinishedEvent);
    }

    //private void HandleAskQuestionEvent(AskQuestionEvent @event)
    //{
    //    //Lesson Quiz interaction for Question and Answers is Handled through the EventMessaging Bus//

    //    Question question = (Question)@event.Question;


    //    if (question.QuestionType == QuestionType.TrueFalse)
    //    {
    //        while (true)
    //        {
    //            Console.WriteLine($"[lime]{question.QuestionText}[/] [yellow](T or F)[/] [lime] ? [/]");
    //            string userInput = Console.ReadLine();
    //            userInput = userInput.Trim().ToLower();

    //            //CBT3_ConsoleHelper.EraseLine();

    //            if (userInput == "t" || userInput == "f")
    //            {
    //                string answerText = userInput == "t" ? "True" : "False";

    //                Answer selectedAnswer = question.Answers.FirstOrDefault(a => a.AnswerText.Equals(answerText, StringComparison.OrdinalIgnoreCase));
    //                selectedAnswer.IsSelected = true;

    //                if (selectedAnswer != null)
    //                {
    //                    //submit to service using messaging bus
    //                    _messenger.Publish(
    //                        new TFQuestionAnsweredEvent(
    //                            DateTime.Now,
    //                            selectedAnswer,
    //                            QuizState.QuestionAnswered,
    //                            $"{selectedAnswer.AnswerText}"));
    //                    break;
    //                }
    //                else
    //                {
    //                    Console.WriteLine(string.Empty);
    //                    Console.ForegroundColor = ConsoleColor.Red;
    //                    int screenWidth = Console.WindowWidth;
    //                    Console.WriteLine(new string('-', screenWidth));
    //                    string message = $"Please enter valid options. either 'T' or 'F':\")";
    //                    Console.WriteLine(message);
    //                    Console.WriteLine(new string('-', screenWidth));
    //                    Console.ResetColor();
    //                    Console.WriteLine(string.Empty);
    //                }
    //            }
    //            else
    //            {
    //                Console.WriteLine(string.Empty);
    //                Console.ForegroundColor = ConsoleColor.Red;
    //                int screenWidth = Console.WindowWidth;
    //                Console.WriteLine(new string('-', screenWidth));
    //                string message = $"Please enter valid options. either 'T' or 'F':\")";
    //                Console.WriteLine(message);
    //                Console.WriteLine(new string('-', screenWidth));
    //                Console.ResetColor();
    //                Console.WriteLine(string.Empty);
    //            }
    //        }
    //    }
    //    else if (question.QuestionType == QuestionType.MultipleChoice)
    //    {
    //        while (true)
    //        {
    //            Console.WriteLine(question.QuestionText);
    //            Console.WriteLine("Choose the correct option(s) by entering the corresponding number(s):");

    //            // Print each option with numbering
    //            for (int i = 0; i < question.Answers.Count; i++)
    //            {
    //                Console.WriteLine($"{i + 1}) {question.Answers[i].AnswerText}");
    //            }

    //            Console.WriteLine("Enter your answers (comma-separated, e.g., 1,3,4):");
    //            string userInput = Console.ReadLine();

    //            //CBT3_ConsoleHelper.EraseLine();

    //            string[] selectedOptions = userInput.Split(',');
    //            List<Answer> selectedAnswers = new List<Answer>();
    //            bool invalidOption = false;
    //            foreach (string option in selectedOptions)
    //            {
    //                if (int.TryParse(option, out int selectedOption) &&
    //                    selectedOption > 0 &&
    //                    selectedOption <= question.Answers.Count)
    //                {
    //                    selectedAnswers.Add(question.Answers[selectedOption - 1]);
    //                }
    //                else
    //                {
    //                    Console.WriteLine($"Invalid option '{option}'. Ignored.");
    //                    invalidOption = true;
    //                }
    //            }
    //            if (invalidOption)
    //            {
    //                // If any invalid option was encountered, ask the user to re-enter their choices
    //                Console.WriteLine(string.Empty);
    //                Console.ForegroundColor = ConsoleColor.Red;
    //                int screenWidth = Console.WindowWidth;
    //                Console.WriteLine(new string('-', screenWidth));
    //                string message = $"Please enter valid options. (comma-separated, e.g., 1,3,4):\")";
    //                Console.WriteLine(message);
    //                Console.WriteLine(new string('-', screenWidth));
    //                Console.ResetColor();
    //                Console.WriteLine(string.Empty);

    //                continue; // Restart the loop to prompt the user again
    //            }
    //            selectedAnswers.ForEach(x => x.IsSelected = true); //need to set it as selected;
    //                                                               //submit to service using messaging bus
    //            _messenger.Publish(
    //                new MCQuestionAnsweredEvent(
    //                    DateTime.Now,
    //                    selectedAnswers,
    //                    QuizState.QuestionAnswered,
    //                    string.Empty));

    //            break;
    //        }
    //    }
    //}

    private void HandleCourseStateEvent(CourseStateEvent @event)
    {
        //Console.WriteLine($"=>{@event.Text} ");
        //If the Course State Event is "CourseFinished", this requires 
        //Using the CQRS / MEDIATOR bus to send a CourseCompletionCommand
        //This command will fire off the _= await _dataService.CompleteCourseAsync(trainee, course, coursepass);
        //Stored proc to record the results and writeout the xml etc.

        //bool coursepass = @event.CoursePass;
        _cbtApp.CoursePass = @event.CoursePass;
        if (@event.State.Equals(CourseState.CourseFinished))
        {
            TraineeID traineeId = new(_cbtApp.Trainee.Id.Value);
            CourseID courseId = new(_cbtApp.Course.Id.Value);

            CourseCompletionCommand request = new CourseCompletionCommand(traineeId, courseId, _cbtApp.CoursePass);

            var result = Task.Run(()=>_mediator.SendAsync<Result<bool>>(request, default)).Result;

            if (result.IsSuccess && _cbtApp.CoursePass)
            {
                _courseMessage = (MarkupString)$"{@event.Text} ";
                _courseMessageStyle = "display:inline";
                _messageStyle = "";
                _messageStyle = "display:none";
                _continueButton = "";
                StateHasChanged();
                return;
            }
            else if (result.IsSuccess && !_cbtApp.CoursePass)
            {
                _courseMessage = (MarkupString)$"<h2>{@event.Text}</h2><br><h2><strong> Please return to the Front Counter for further instructions</h2></strong>";
                _courseMessageStyle = "display:inline";
                _messageStyle = "";
                _messageStyle = "display:none";
                _continueButton = "display:none";
                StateHasChanged();
                return;
            }





        }
    }

    private async void HandleLessonPageEvent(LessonPageEvent @event)
    {
        
        //This determines what page strategy to use//Text/Video//Custom Page//Quiz//
        PageType pt = @event.Page.LessonPageType;
        LessonPage lessonPage = @event.Page;
        AppState.SetProperty(this, "LessonPage", lessonPage);

        if (pt.Equals(PageType.PT_BASE_01)) //COURSE_INTRO
        {

            //_courseMessage = (MarkupString)$"=>{@event.Text} {lessonPage.PageOrder} {lessonPage.LessonPageSubType} ";
            _courseMessage = (MarkupString)$"<h2><strong>{lessonPage.PageText}</strong></h2>";
            //headingmessage = (MarkupString)lessonPage.PageText;
            _courseMessageStyle = "display:inline";
            _messageStyle = "";
            _messageStyle = "display:none";
            _continueButton = "";
            StateHasChanged();
            return;

        }
        else if (pt.Equals(PageType.PT_BASE_02)) //LESSON_OVERVIEW
        {
            //_courseMessage = (MarkupString)$"=>{@event.Text} {lessonPage.PageOrder} {lessonPage.LessonPageSubType} ";
            _courseMessage = (MarkupString)$"<h2><strong>{lessonPage.PageText}</strong></h2>";
            
            _courseMessageStyle = "display:inline";
            _messageStyle = "";
            _messageStyle = "display:none";
            _continueButton = "";
            StateHasChanged();
            return;
            
        }
        else if (pt.Equals(PageType.PT_BASE_03)) //KNOWLEDGE_CHECK
        {
            //_courseMessage = (MarkupString)$"=>{@event.Text} {lessonPage.PageOrder} {lessonPage.LessonPageSubType} ";
            _courseMessage = (MarkupString)$"<h2><strong>{lessonPage.PageText}</strong></h2>";
            
            _courseMessageStyle = "display:inline";
            _messageStyle = "";
            _messageStyle = "display:none";
            



            _continueButton = "";
            StateHasChanged();
            return;
           
        }
        else if (pt.Equals(PageType.PT_BASE_04)) //COURSE_END
        {
            //_courseMessage = (MarkupString)$"=>{@event.Text} {lessonPage.PageOrder} {lessonPage.LessonPageSubType} ";
            _courseMessage = (MarkupString)$"<h2><strong>{lessonPage.PageText}</strong></h2>";

            _courseMessageStyle = "display:inline";
            _messageStyle = "";
            _messageStyle = "display:none";
            _continueButton = "";
            if (_cbtApp.CoursePass)
            {
                _coursecompleted = "display: flex; gap: 10px;";
            }
            else
            {
                _coursecompleted = "display:none";
            }
            StateHasChanged();
            return;

        }
        else if (pt.Equals(PageType.PT_BASE_05)) //VIDEO
        {
            if (lessonPage.VideoURL != null)
            {
                if (CheckFileExists("./video/" + lessonPage.VideoURL))
                {
                    _courseMessage = (MarkupString)"";
                    _courseMessageStyle = "display:none";
                    _messageStyle = "";
                    _messageStyle = "display:none";
                    _videoFileName = "./video/" + lessonPage.VideoURL;
                    var result = await ShowVideo(_videoFileName);

                    //if (result.IsSuccess)
                    //{
                    //    _courseMessage = (MarkupString)"<b> VIDEO FINISHED </b>";
                    //    _courseMessageStyle = "";
                    //}
                    ContinueClick();
                    //StateHasChanged();
                    //return;
                }
            }
            else
            {
                _courseMessage = (MarkupString)"<b> VIDEO MISSING </b>";
                _courseMessageStyle = "";
                StateHasChanged();
                return;
            }
        }
        else if(pt.Equals(PageType.PT_BASE_06)) //QUIZ
        {
            //_courseMessage = (MarkupString)$"=>{@event.Text} {lessonPage.PageOrder} {lessonPage.LessonPageSubType} ";
            _courseMessage = (MarkupString)$"{lessonPage.PageText}";
            
            _courseMessageStyle = "display:inline";
            _messageStyle = "";
            _messageStyle = "display:none";
            _continueButton = "";
            StateHasChanged();
            return;
            
        }

        
        else if (pt.Equals(PageType.PT_BASE_07)) //COURSE_PASS
        {
            //_courseMessage = (MarkupString)$"=>{@event.Text} {lessonPage.PageOrder} {lessonPage.LessonPageSubType} ";
            _courseMessage = (MarkupString)$"<h2><strong>{lessonPage.PageText}</strong></h2>";
            
            _courseMessageStyle = "display:inline";
            _messageStyle = "";
            _messageStyle = "display:none";
            _continueButton = "";
            StateHasChanged();
            return;
        }
        else if (pt.Equals(PageType.PT_BASE_08)) //COURSE_FAIL
        {
            //_courseMessage = (MarkupString)$"=>{@event.Text} {lessonPage.PageOrder} {lessonPage.LessonPageSubType} ";
            _courseMessage = (MarkupString)$"<h2><strong>{lessonPage.PageText}</strong></h2>";
            
            _courseMessageStyle = "display:inline";
            _messageStyle = "";
            _messageStyle = "display:none";
            _continueButton = "";
            StateHasChanged();
            return;
        }


    }

    private void HandleMachinePauseEvent(MachinePauseEvent @event)
    {
        //The nested Machines (Course=> Lesson=> LessonPage) requires evalutaing which machine raises the 
        //Machine Pause event//
        //Console.ReadLine();
         _currentState = @event.State;
        // Console.WriteLine(state.Value);
        //if (state.Equals(MachineState.CourseMachinePaused))
        //{
        //    MachineResumeCommand request = new MachineResumeCommand(CBT3_App.CourseMachineSvc);
        //    _ = Mediator.SendAsync(request, default);
        //}
        //if (state.Equals(MachineState.LessonMachinePaused))
        //{
        //    MachineResumeCommand request = new MachineResumeCommand(CBT3_App.CourseMachineSvc.ChildMachine);
        //    _ = Mediator.SendAsync(request, default);
        //}
        //if (state.Equals(MachineState.LessonPageMachinePaused))
        //{
        //    MachineResumeCommand request = new MachineResumeCommand(CBT3_App.CourseMachineSvc.ChildMachine.ChildMachine);
        //    _ = Mediator.SendAsync(request, default);
        //}
    }

    private void HandleLessonQuizFinishedEvent(LessonQuizFinishedEvent @event)
    {
        bool isFail = @event.IsFail;

        //if (isFail)
        //{
        //    //_isQuizFail = true;
        //    Console.ForegroundColor = ConsoleColor.Red;
        //    int screenWidth = Console.WindowWidth;
        //    Console.WriteLine(new string('-', screenWidth));
        //    string message = $"Quiz failed! You exceeded the maximum attempts allowed.";
        //    Console.WriteLine(message);
        //    Console.WriteLine(new string('-', screenWidth));
        //    Console.ResetColor();
        //}
        //else
        //{
        //    //_isQuizFail = false;
        //    Console.ForegroundColor = ConsoleColor.Blue;
        //    int screenWidth = Console.WindowWidth;
        //    Console.WriteLine(new string('-', screenWidth));
        //    string message = $"Congratulations! Quiz completed successfully.";
        //    Console.WriteLine(message);
        //    Console.WriteLine(new string('-', screenWidth));
        //    Console.ResetColor();
        //}

        //_messenger.Unsubscribe<AskQuestionEvent>(HandleAskQuestionEvent);
    }

    private void HandleLessonQuizStartedEvent(LessonQuizStartedEvent @event)
    {
        //Quiz Started State Raises the QuizState.QuizStarted event
        //The client needs to subscribe to the "HandleAskQuestionEvents"
        _courseMessage = (MarkupString)$"";
        _courseMessageStyle = "display:none";
        _messageStyle = "";
        _messageStyle = "display:none";
        _continueButton = "";
        StateHasChanged();
        LessonQuiz quiz = @event.LessonQuiz;
        Trainee trainee = _cbtApp.Trainee;
        CourseID courseid = new (_courseId);

         ShowQuiz(trainee, courseid, quiz);
       
       
    }

    private void CourseMachineResume()
    {
        //message = (MarkupString)"";
        var request = new MachineResumeCommand(_courseMachine)
        {
            Machine = _courseMachine

        };
        var success = _mediator.SendAsync(request,default).Result;

        InvokeAsync(StateHasChanged);
    }
    private void LessonMachineResume()
    {
        //message = (MarkupString)"";
        var request = new MachineResumeCommand(_courseMachine)
        {
            Machine = _courseMachine.ChildMachine

        };
        var success = _mediator.SendAsync(request,default).Result;

        InvokeAsync(StateHasChanged);
    }
    private void LessonPageMachineResume()
    {
        //message = (MarkupString)"";
        var request = new MachineResumeCommand(_courseMachine)
        {
            Machine = _courseMachine.ChildMachine.ChildMachine

        };
        var success = _mediator.SendAsync(request,default).Result;

        InvokeAsync(StateHasChanged);
    }

    private bool CheckFileExists(string filePath)
    {

        bool fileExists = _systemService.IsFileExists(filePath).Result.IsSuccess;
        return fileExists;
    }
}
