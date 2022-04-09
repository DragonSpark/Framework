using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Application.Connections.Client;

public interface IReceiver : IOperation, IAsyncDisposable {}