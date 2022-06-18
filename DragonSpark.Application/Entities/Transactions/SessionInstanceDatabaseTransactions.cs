namespace DragonSpark.Application.Entities.Transactions;

public sealed class SessionInstanceDatabaseTransactions : AppendedTransactions
{
	public SessionInstanceDatabaseTransactions(ScopedEntityContextTransactions first,
	                                           EstablishSessionTransactions second)
		: base(first, second) {}
}