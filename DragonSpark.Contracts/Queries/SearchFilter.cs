using System.Collections.Generic;

namespace DragonSpark.Contracts.Queries;

public sealed record SearchFilter(
	IReadOnlyCollection<string> Fields,
	string Key,
	string Operator,
	bool IgnoreCase,
	bool IgnoreAccent);