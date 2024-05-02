using CBT3_Domain.Entities;
using CBT3_Domain.Errors;

namespace CBT3_Application.Messaging.CommandHandlers;


    public class AddTraineeCommandHandler : BaseCommandBundle,IRequestHandler<AddTraineeCommand, Result<Trainee>>
    {
        private readonly RegistrationService _registrationService;
        public AddTraineeCommandHandler(RegistrationService registrationService)
        {
        _registrationService = registrationService;
        }
        
        public async Task<Result<Trainee>> HandleAsync(AddTraineeCommand request, CancellationToken ct = default)
        {
            request.Trainee.CreatedDate = DateTime.UtcNow;
            
            Result<Trainee> traineeResult = await _registrationService.AddTraineeAsync(request.Trainee,ct).ConfigureAwait(false);

            if (traineeResult.IsSuccess)
            {
                return traineeResult;
            }
            else
            {
                return Result<Trainee>.Failure<Trainee>(traineeResult.Error);

            }
        
        }
    }

