namespace CBT3_Application.Messaging.CommandHandlers;


    public class MachineStateExitCommandHandler : BaseCommandBundle, IRequestHandler<MachineStateExitCommand, IBaseMachine>
    {
    public MachineStateExitCommandHandler()
    {
        
    }
    
    public Task<IBaseMachine> HandleAsync(MachineStateExitCommand request, CancellationToken ct = default)
        {
            request.Machine.CurrentState.ExitState(true);
            return Task.FromResult(request.Machine);
        }
    }

