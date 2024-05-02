namespace CBT3_Application.Services;

public sealed class MessengerService : IMessenger
{
    private readonly Dictionary<Type, List<object>> _subscriptions = new Dictionary<Type, List<object>>();

    public void Subscribe<TEvent>(Action<TEvent> action)
    {
        var eventType = typeof(TEvent);
        if (!_subscriptions.ContainsKey(eventType))
        {
            _subscriptions[eventType] = new List<object>();
        }
        _subscriptions[eventType].Add(action);
    }
    public void Unsubscribe<TEvent>(Action<TEvent> action)
    {
        var eventType = typeof(TEvent);
        if (_subscriptions.ContainsKey(eventType))
        {
            _subscriptions[eventType].Remove(action);
        }
    }
    public void Publish<TEvent>(TEvent @event)
    {
        //Console.WriteLine($"EventAggregator Publishing Event Type {@event.GetType().ToString()}");
        var eventType = typeof(TEvent);
        if (_subscriptions.ContainsKey(eventType))
        {
            foreach (var subscriber in _subscriptions[eventType])
            {
                ((Action<TEvent>)subscriber)(@event);
            }
        }
    }
}
