using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Application.Connections.Events;

public interface ISubscription : IOperation, IAsyncDisposable;