namespace DragonSpark.Application.Entities.Transactions;

public sealed class SessionInstanceCurrentDatabaseTransactions : AppendedTransactions
{
	public SessionInstanceCurrentDatabaseTransactions(ScopedEntityContextTransactions first,
	                                                  EstablishSessionCurrentTransactions second)
		: base(first, second) {}
}