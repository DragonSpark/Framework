﻿using DragonSpark.Application.Runtime.Operations;

namespace DragonSpark.SyncfusionRendering.Queries;

public sealed class Reporter : IReporter<IDataRequest>
{
	public static Reporter Default { get; } = new();

	Reporter() {}

	public IDataRequest Get(Report<IDataRequest> parameter)
	{
		var (previous, reporter) = parameter;
		return new ReportedDataRequest(previous, reporter);
	}
}