//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//     Author: Scott Green
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using CBT3_Application.Messaging;
using CBT3_Application.Messaging.Commands;
using CBT3_Application.Messaging.Queries;
using CBT3_Application.Services;

using CBT3_Domain.Common;
using CBT3_Domain.Entities;
using CBT3_Domain.Errors;
using CBT3_Domain.ValueObjects;

namespace CBT3_UI;

public class CBT3_App
{
    //public static CourseMachine? CourseMachineSvc { get; set; } = null;
    public Trainee? Trainee { get; set; } = null;
    public Course? Course { get; set; } = null;
    public LessonPage? LessonPage { get; set; } = null;
    public bool CoursePass { get; set; } = false;

    public string Session { get; set; } = string.Empty;


    //public static void InitializeTrainee()
    //{
    //    TraineeID traineeID = new(Guid.NewGuid().ToString());
    //    Trainee = new(traineeID);

    //}
    //public static async Task<Result<Trainee>> AddTraineeAsync(Trainee _trainee)
    //{
    //    try
    //    {
    //        AddTraineeCommand newtrainee_cmd = new AddTraineeCommand { Trainee = _trainee };

    //        var result = await CBT3_EventHandlers.Mediator.SendAsync(newtrainee_cmd, default);
    //        return result;
    //    }
    //    catch (Exception ex)
    //    {
    //        // Handle exceptions here
    //        Console.WriteLine($"An error occurred while adding trainee: {ex.Message}");
    //        return Result.Failure<Trainee>(DomainErrors.TraineeError.NullOrEmpty);
    //    }
    //}
    //public static async Task<Result<YearOfBirth>> GetYearOfBirthAsync()
    //{
    //    string yearOfBirth = string.Empty;
    //    YearOfBirth _yearOfBirth = null;

    //    while (true)
    //    {
    //        Console.WriteLine("[lime]Enter your 4 digit Year of Birth:[/]");
    //        yearOfBirth = Console.ReadLine();


    //        try
    //        {

    //            SubmitFirstYearOfBirthCommand submitfirstYearOfBirthCmd = new SubmitFirstYearOfBirthCommand
    //            {
    //                FirstYearOfBirth = yearOfBirth
    //            };

    //            var first_yearOfBirthResult = await CBT3_EventHandlers.Mediator.SendAsync(submitfirstYearOfBirthCmd, default);

    //            if (first_yearOfBirthResult.IsSuccess)
    //            {
    //                Console.WriteLine("[lime]Please re-enter the same Year of Birth:[/]");
    //                string reenteredYearOfBirth = Console.ReadLine();
    //                SubmitSecondYearOfBirthCommand submitsecondYearOfBirthCmd = new SubmitSecondYearOfBirthCommand
    //                {
    //                    FirstYearOfBirth = first_yearOfBirthResult.Value,
    //                    SecondYearOfBirth = reenteredYearOfBirth
    //                };

    //                var second_yearOfBirthResult = await CBT3_EventHandlers.Mediator.SendAsync(submitsecondYearOfBirthCmd, default);

    //                if (second_yearOfBirthResult.IsSuccess)
    //                {
    //                    _yearOfBirth = second_yearOfBirthResult.Value;
    //                    break;
    //                }
    //                else
    //                {
    //                    Console.WriteLine(second_yearOfBirthResult.Error.Message);
    //                }
    //            }
    //            else
    //            {
    //                Console.WriteLine(first_yearOfBirthResult.Error.Message);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"An error occurred: {ex.Message}");
    //        }
    //    }

    //    return _yearOfBirth;
    //}
    //public static async Task<Result<UPID>> GetUPIDAsync()
    //{
    //    string upid = string.Empty;
    //    UPID _upid = null;

    //    while (true)
    //    {
    //        Console.WriteLine("[lime]Enter your 7 digit UPID:[/]");
    //        upid = Console.ReadLine();


    //        try
    //        {
    //            SubmitFirstUPIDCommand submitfirstUPIDCmd = new SubmitFirstUPIDCommand
    //            {
    //                FirstUPID = upid
    //            };

    //            var first_upidResult = await CBT3_EventHandlers.Mediator.SendAsync(submitfirstUPIDCmd, default);

    //            if (first_upidResult.IsSuccess)
    //            {
    //                Console.WriteLine("[lime]Please re-enter the same UPID:[/]");
    //                string reenteredUpid = Console.ReadLine();
    //                SubmitSecondUPIDCommand submitsecondUPIDCmd = new SubmitSecondUPIDCommand
    //                {
    //                    FirstUPID = first_upidResult.Value,
    //                    SecondUPID = reenteredUpid
    //                };

    //                var second_upidResult = await CBT3_EventHandlers.Mediator.SendAsync(submitsecondUPIDCmd, default);

    //                if (second_upidResult.IsSuccess)
    //                {
    //                    _upid = second_upidResult.Value;
    //                    break;
    //                }
    //                else
    //                {
    //                    Console.WriteLine(second_upidResult.Error.Message);
    //                }
    //            }
    //            else
    //            {
    //                Console.WriteLine(first_upidResult.Error.Message);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"An error occurred: {ex.Message}");
    //        }
    //    }

    //    return _upid;
    //}
    //public static async Task<Result<LastName>> GetLastNameAsync()
    //{
    //    string lastName = string.Empty;
    //    LastName _lastName = null;

    //    while (true)
    //    {
    //        Console.WriteLine("[lime]Enter your Last name:[/]");
    //        lastName = Console.ReadLine();


    //        try
    //        {
    //            SubmitLastNameCommand submitlastname = new SubmitLastNameCommand
    //            {
    //                LastName = lastName
    //            };

    //            var lastnameResult = await CBT3_EventHandlers.Mediator.SendAsync(submitlastname, default);

    //            if (lastnameResult.IsSuccess)
    //            {
    //                _lastName = lastnameResult.Value;
    //                break;
    //            }
    //            else
    //            {
    //                Console.WriteLine(lastnameResult.Error.Message);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"An error occurred: {ex.Message}");
    //        }
    //    }

    //    return _lastName;
    //}
    //public static async Task<Result<FirstName>> GetFirstNameAsync()
    //{
    //    string firstName = string.Empty;
    //    FirstName _firstName = null;

    //    while (true)
    //    {
    //        Console.WriteLine("[bold][lime]Enter your First name:[/][/]");
    //        firstName = Console.ReadLine();


    //        try
    //        {
    //            SubmitFirstNameCommand submitfirstname = new SubmitFirstNameCommand
    //            {
    //                FirstName = firstName
    //            };

    //            var firstnameResult = await CBT3_EventHandlers.Mediator.SendAsync(submitfirstname, default);

    //            if (firstnameResult.IsSuccess)
    //            {
    //                _firstName = firstnameResult.Value;
    //                await Task.Delay(2); // Delay is asynchronous now
    //                break;
    //            }
    //            else
    //            {
    //                Console.WriteLine(firstnameResult.Error.Message);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"An error occurred: {ex.Message}");
    //        }
    //    }

    //    return _firstName;
    //}

    //private static async Task<Result<List<string>>> GetCourseCodesAsync(bool sidaOnly)
    //{
    //    try
    //    {
    //        bool sidaonly = sidaOnly;

    //        GetCourseCodesQuery coursecodesQuery = new GetCourseCodesQuery(CBT3_App.Trainee, sidaonly);

    //        return await CBT3_EventHandlers.Mediator.SendAsync(coursecodesQuery, default);
    //    }
    //    catch (Exception ex)
    //    {
    //        // Handle exceptions here
    //        Console.WriteLine($"An error occurred while getting course codes: {ex.Message}");
    //        return Result.Failure<List<string>>(DomainErrors.GeneralError.UnProcessableRequest);
    //    }
    //}
    //public static async Task SelectCourseAsync()
    //{
    //    List<string> coursecode_list = GetCourseCodesAsync(true).Result.Value;

    //    while (true)
    //    {
    //        Console.WriteLine("Choose a Course option:");
    //        for (int i = 0; i < coursecode_list.Count; i++)
    //        {
    //            Console.WriteLine($"{i + 1}. {coursecode_list[i]}");
    //        }
    //        Console.Write("Enter the number of your choice: ");
    //        string userInput = Console.ReadLine();
    //        //string userInput = AnsiConsole.Prompt(
    //        //    new SelectionPrompt<string>()
    //        //        .Title("[lime]Choose a Course option[/]?")
    //        //        .PageSize(6)
    //        //        .AddChoices(coursecode_list));
    //        string selectedCourse = string.Empty;
    //        if (int.TryParse(userInput, out int choice) && choice > 0 && choice <= coursecode_list.Count)
    //        {
    //            selectedCourse = coursecode_list[choice - 1];
    //            // Proceed with courseId
    //        }
    //        CourseID courseId = new(selectedCourse);
    //        GetCourseQuery courseQuery = new(courseId);
    //        Result<Course> result = await CBT3_EventHandlers.Mediator.SendAsync(courseQuery, default);

    //        if (result.IsSuccess)
    //        {
    //            Course = result.Value;
    //            break;
    //        }
    //        else
    //        {
    //            Console.WriteLine("Failed to get course. Please try again.");
    //        }
    //    }
    //}
    //private static async Task InitializeCourseMachine()
    //{

    //    CourseMachineSvc = new CourseMachine(CBT3_EventHandlers.Mediator, CBT3_EventHandlers.Messenger);
    //    CourseMachineSvc.InitializeMachine(Trainee, Course);
    //    MachineStartCommand request = new MachineStartCommand(CourseMachineSvc);
    //    var result = await CBT3_EventHandlers.Mediator.SendAsync(request, default);
    //}
    //public static async Task StartCourseAsync()
    //{
    //    if (Trainee is not null && Course is not null)
    //    {

    //        /// <summary>
    //        /// Get the CourseMachine and Initialize CQRS / MEDIATOR
    //        /// </summary>
    //        /// 
    //        await InitializeCourseMachine();
    //    }
    //    else
    //    {
    //        Console.WriteLine("Better go back..");
    //    }
    //}

    //public static Result<bool> SelectAnotherCourse()
    //{
    //    Console.WriteLine($"[lime]{"Another Course?"}[/] [yellow](T or F)[/] [lime] ? [/]");
    //    string userInput = Console.ReadLine();
    //    userInput = userInput.Trim().ToLower();

    //    if (userInput == "t" )
    //    {
    //        var course = CBT3_App.SelectCourseAsync();
    //        EraseLine();
    //        var play = CBT3_App.StartCourseAsync();
    //        return Result.Success(true);
    //    }
    //    else if (userInput == "f")
    //    {
    //        return Result.Success(false);
    //    }
    //    return Result.Success(false);
    //}
    //public static Task<Result<bool>> FinishCourseAsync()
    //{
    //    CourseMachineSvc = null;
    //    Course = null;
    //    return Task.FromResult(Result.Success(true));
    //}





}
