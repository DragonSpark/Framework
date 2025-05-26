using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

public interface ITransactions : IStopAware<ITransaction>;