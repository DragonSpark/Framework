namespace DragonSpark.Application.Entities.Transactions;

public sealed class EstablishSessionTransactions : AppendedTransactions
{
	public EstablishSessionTransactions(SessionBoundaryTransactions first, DatabaseTransactions second)
		: base(first, second) {}
}