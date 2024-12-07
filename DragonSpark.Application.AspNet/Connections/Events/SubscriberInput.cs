using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Events;

public readonly record struct SubscriberInput(uint? Recipient, Func<Task> Body);

public readonly record struct SubscriberInput<T>(uint? Recipient, Func<T, Task> Body);