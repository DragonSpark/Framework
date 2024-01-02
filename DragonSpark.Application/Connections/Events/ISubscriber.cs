using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Connections.Events;

public interface ISubscriber<T> : ISelect<SubscriberInput<T>, ISubscription>;

public interface ISubscriber : ISelect<SubscriberInput, ISubscription>;