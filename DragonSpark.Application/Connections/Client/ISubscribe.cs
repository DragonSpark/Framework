using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

// TODO: Move
public interface ISubscribe : ISelect<Func<Task>, ISubscription>;

public interface ISubscribe<out T> : ISelect<Func<T, Task>, ISubscription>;

public readonly record struct SubscriberInput<T>(uint? Recipient, Func<T, Task> Body);

public readonly record struct SubscriberInput(uint? Recipient, Func<Task> Body);

public interface ISubscriber : ISelect<SubscriberInput, ISubscription>;

public interface ISubscriber<T> : ISelect<SubscriberInput<T>, ISubscription>;