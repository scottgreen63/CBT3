namespace CBT3_Application.Messaging;

    public class MachineStartCommandHandler : BaseCommandBundle,IRequestHandler<MachineStartCommand, IBaseMachine>
    {
        public MachineStartCommandHandler()
        {
           
        }
        public Task<IBaseMachine> HandleAsync(MachineStartCommand request, CancellationToken ct = default)
        {
            request.Machine.Start();
            return Task.FromResult(request.Machine);
        }
    }

