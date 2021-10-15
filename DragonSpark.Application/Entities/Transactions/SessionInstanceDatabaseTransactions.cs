namespace DragonSpark.Application.Entities.Transactions;

public class SessionInstanceDatabaseTransactions : AppendedTransactions
{
	public SessionInstanceDatabaseTransactions(SessionInstanceTransactions first,
	                                           SessionDatabaseTransactions second)
		: base(first, second) {}
}