using DragonSpark.Model.Selection.Alterations;
using System;
using System.Linq;

namespace DragonSpark.Application.Diagnostics;

public sealed class Aggregation : IAlteration<Exception>
{
	public static Aggregation Default { get; } = new();

	Aggregation() {}

	public Exception Get(Exception parameter)
		=> parameter is AggregateException aggregate ? aggregate.InnerExceptions.First() : parameter;
}