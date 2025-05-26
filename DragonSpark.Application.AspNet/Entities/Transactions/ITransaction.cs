using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Stop;
using System;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

public interface ITransaction : ICommand, IStopAware, IAsyncDisposable;