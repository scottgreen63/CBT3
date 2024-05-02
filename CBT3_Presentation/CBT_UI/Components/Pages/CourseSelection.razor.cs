using System.Net.NetworkInformation;

using CBT_UI.Components.Pages.Shared;

using CBT3_Application.Interfaces;
using CBT3_Application.Messaging;
using CBT3_Application.Messaging.Queries;
using CBT3_Application.Services;

using CBT3_Domain.Common;
using CBT3_Domain.Entities;
using CBT3_Domain.Errors;

using CBT3_UI;

using Microsoft.AspNetCore.Components;

namespace CBT_UI.Components.Pages;

public partial class CourseSelection : ComponentBase
{
    [CascadingParameter]
    public CascadingAppState AppState { get; set; }
    [Inject]
    protected CBT3_App _cbtApp { get; set; }

    public CourseMachine? CourseMachineSvc { get; set; } = null;

    private async Task<Result<List<string>>> GetCourseCodesAsync(bool sidaOnly)
    {
        try
        {
            bool sidaonly = sidaOnly;

            GetCourseCodesQuery coursecodesQuery = new GetCourseCodesQuery(_cbtApp.Trainee, sidaonly);

            return await _mediator.SendAsync(coursecodesQuery, default);
        }
        catch (Exception ex)
        {
            // Handle exceptions here
            Console.WriteLine($"An error occurred while getting course codes: {ex.Message}");
            return Result.Failure<List<string>>(DomainErrors.GeneralError.UnProcessableRequest);
        }
    }
    public async Task SelectCourseAsync()
    {
        List<string> coursecode_list = GetCourseCodesAsync(true).Result.Value;

        while (true)
        {
            Console.WriteLine("Choose a Course option:");
            for (int i = 0; i < coursecode_list.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {coursecode_list[i]}");
            }
            Console.Write("Enter the number of your choice: ");
            string userInput = Console.ReadLine();
            //string userInput = AnsiConsole.Prompt(
            //    new SelectionPrompt<string>()
            //        .Title("[lime]Choose a Course option[/]?")
            //        .PageSize(6)
            //        .AddChoices(coursecode_list));
            string selectedCourse = string.Empty;
            if (int.TryParse(userInput, out int choice) && choice > 0 && choice <= coursecode_list.Count)
            {
                selectedCourse = coursecode_list[choice - 1];
                // Proceed with courseId
            }
            CourseID courseId = new(selectedCourse);
            GetCourseQuery courseQuery = new(courseId);
            Result<Course> result = await _mediator.SendAsync(courseQuery, default);

            if (result.IsSuccess)
            {
                _cbtApp.Course = result.Value;
                break;
            }
            else
            {
                Console.WriteLine("Failed to get course. Please try again.");
            }
        }
    }

    public async Task StartCourseAsync()
    {
        if (_cbtApp.Trainee is not null && _cbtApp.Course is not null)
        {

            /// <summary>
            /// Get the CourseMachine and Initialize CQRS / MEDIATOR
            /// </summary>
            ///
            await InitializeCourseMachine();
        }
        else
        {
            Console.WriteLine("Better go back..");
        }
    }

    
    private async Task InitializeCourseMachine()
    {

        CourseMachineSvc = new CourseMachine(_mediator, _messenger);
        CourseMachineSvc.InitializeMachine(_cbtApp.Trainee, _cbtApp.Course);
        MachineStartCommand request = new MachineStartCommand(CourseMachineSvc);
        var result = await _mediator.SendAsync(request, default);
    }

    private void UpdateToolBarMessage(string message)
    {
        AppState.SetProperty(this, "Course", message);
    }
}
