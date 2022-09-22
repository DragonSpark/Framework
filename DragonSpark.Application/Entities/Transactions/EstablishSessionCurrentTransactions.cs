namespace DragonSpark.Application.Entities.Transactions;

public sealed class EstablishSessionCurrentTransactions : AppendedTransactions
{
	public EstablishSessionCurrentTransactions(SessionBoundaryTransactions first,
	                                           SessionCurrentDatabaseTransactions second)
		: base(first, second) {}
}