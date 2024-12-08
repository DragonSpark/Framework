using System;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

public interface IScopedTransaction : ITransaction
{
	IServiceProvider Provider { get; }
}