using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Application.Entities.Transactions;

public interface ITransaction : ICommand, IOperation, IAsyncDisposable {}