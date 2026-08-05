namespace DragonSpark.Contracts.Queries;

public sealed record WhereFilter(
	string Field,
	bool IgnoreCase,
	bool IgnoreAccent,
	bool IsComplex,
	string Operator,
	string Condition,
	object? value,
	IReadOnlyCollection<WhereFilter> predicates);