using System;

namespace DragonSpark.Application.Entities.Transactions;

public interface IScopedTransaction : ITransaction
{
	IServiceProvider Provider { get; }
}