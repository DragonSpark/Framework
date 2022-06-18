namespace DragonSpark.Application.Entities.Transactions;

public sealed class EstablishSessionTransactions : AppendedTransactions
{
	public EstablishSessionTransactions(SessionBoundaryTransactions first, SessionDatabaseTransactions second)
		: base(first, second) {}
}