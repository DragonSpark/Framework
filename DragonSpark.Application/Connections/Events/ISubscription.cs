using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Connections.Events;

public interface ISubscription : IOperation, IAsyncDisposable;