//using CBT3_Domain.Events.DomainEvents;
using CBT_Infrastructure.Services;

using CBT3_Domain.Common;
using CBT3_Domain.Entities;

namespace CBT3_Application.Services;


public  class RegistrationService
{
    private FirstName? _firstName;
    private LastName? _lastName;
    private UPID? _upid;
    private YearOfBirth? _yearOfBirth;

    private bool _FirstNameCompleted;
    private bool _LastNameCompleted;
    private bool _UPIDCompleted;
    private bool _YOBCompleted;

    private readonly TrainingDataService _trainingDataService;
    public RegistrationService(TrainingDataService trainingDataService)
    {
        _trainingDataService = trainingDataService;
    }
    public async Task<Result<FirstName>> SubmitFirstNameAsync(string firstName, CancellationToken token)
    {
        // Validate first and last name
        Result<FirstName> result = FirstName.Create(firstName);
        // Simulate async operation
        await Task.Delay(0).ConfigureAwait(false);
        if (result.IsSuccess)
        {
            this._firstName = result.Value;
            _FirstNameCompleted = true;
            return Result<FirstName>.Success(result.Value);
        }
        else
        {
            return Result<FirstName>.Failure< FirstName>(result.Error);
        }


        //_NameCompleted = true;
        //return Result.Success(true);
    }

    public async Task<Result<LastName>> SubmitLastNameAsync(string lastName, CancellationToken token)
    {
        // Validate last name
        Result<LastName> result = LastName.Create(lastName);
        await Task.Delay(0).ConfigureAwait(false);
        if (result.IsSuccess)
        {
            this._lastName = result.Value;
            _LastNameCompleted = true;
            return Result<LastName>.Success(result.Value);
        }
        else
        {
            return Result<LastName>.Failure<LastName>(result.Error);
        }
    }

    public async Task<Result<UPID>> SubmitFirstUPIDAsync(string upid, CancellationToken token)
    {
        // Validate last name
        Result<UPID> result = UPID.Create(upid);
        await Task.Delay(0).ConfigureAwait(false);
        if (result.IsSuccess)
        {
            this._upid = result.Value;
            _UPIDCompleted = true;
            return Result<UPID>.Success(result.Value);
        }
        else
        {
            return Result<UPID>.Failure<UPID>(result.Error);
        }

    }
    public async Task<Result<UPID>> SubmitSecondUPIDAsync(string upid, UPID originalupid, CancellationToken token)
    {
        // Validate last name
        Result<UPID> result = UPID.Create(upid, originalupid);
        await Task.Delay(0).ConfigureAwait(false);
        if (result.IsSuccess && _UPIDCompleted)
        {
            this._upid = result.Value;
            return Result<UPID>.Success(result.Value);
        }
        else
        {
            return Result<UPID>.Failure<UPID>(result.Error);
        }

    }

    public async Task<Result<YearOfBirth>> SubmitFirstYearOfBirthAsync(string yearofbirth, CancellationToken token)
    {
        // Validate last name
        Result<YearOfBirth> result = YearOfBirth.Create(yearofbirth);
        await Task.Delay(0).ConfigureAwait(false);
        if (result.IsSuccess)
        {
            this._yearOfBirth = result.Value;
            _YOBCompleted = true;
            return Result<YearOfBirth>.Success(result.Value);
        }
        else
        {
            return Result<YearOfBirth>.Failure<YearOfBirth>(result.Error);
        }

    }
    public async Task<Result<YearOfBirth>> SubmitSecondYearOfBirthAsync(string secondyearofbirth, YearOfBirth originalyearofbirth, CancellationToken token)
    {
        // Validate last name
        Result<YearOfBirth> result = YearOfBirth.Create(secondyearofbirth, originalyearofbirth);
        await Task.Delay(0).ConfigureAwait(false);
        if (result.IsSuccess && _YOBCompleted)
        {
            this._yearOfBirth = result.Value;
            return Result<YearOfBirth>.Success(result.Value);
        }
        else
        {
            return Result<YearOfBirth>.Failure<YearOfBirth>(result.Error);
        }

    }

    public async Task<Result<Trainee>> AddTraineeAsync(Trainee trainee, CancellationToken ct =default)
    {
        return await _trainingDataService.AddTraineeAsync(trainee, ct).ConfigureAwait(false);
    }

    public async Task<Result<Trainee>> GetTraineeByIdAsync(TraineeID traineeID,CancellationToken ct = default)
    {
        return await _trainingDataService.GetTraineeByIdAsync(traineeID, ct);
    }
    //public async Task<Result<Trainee>> GetTraineeAsync(CancellationToken ct = default)
    //{
    //    // Check if all steps are completed
    //    if (_FirstNameCompleted && _LastNameCompleted && _UPIDCompleted && _YOBCompleted)
    //    {
    //        Trainee trainee;

    //        var vfirstNameResult = FirstName.Create(_firstName); // Create a FirstName instance
    //        var vlastNameResult = LastName.Create(_lastName);    // Create a LastName instance
    //        var vupid = UPID.Create(_upid);                    // Create a UPID instance
    //        var vyearOfBirth = YearOfBirth.Create(_yearOfBirth);


    //        // Create and return Trainee object
    //        TraineeID traineeID = new(Guid.NewGuid().ToString());
    //        trainee = new Trainee(traineeID)
    //        {
    //            FirstName = vfirstNameResult.Value,
    //            LastName = vlastNameResult.Value,
    //            UPID = vupid.Value,
    //            YearOfBirth = vyearOfBirth.Value
    //        };
            
    //        return Result<Trainee>.Success(trainee);
    //    }
    //    else
    //    {
    //        return Result<Trainee>.Failure<Trainee>(DomainErrors.TraineeError.NullOrEmpty);
    //    }
    //}

}









