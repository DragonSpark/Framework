namespace DragonSpark.Application.Entities.Transactions;

public sealed class SessionInstanceDatabaseTransactions : AppendedTransactions
{
	public SessionInstanceDatabaseTransactions(ScopedAmbientComponentsTransaction first,
	                                           EstablishSessionTransactions second)
		: base(first, second) {}
}