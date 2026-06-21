using System;

namespace DragonSpark.Presentation.Components.Client;

public sealed class AccessProtectedStorageWaitTime : DragonSpark.Model.Results.Instance<TimeSpan>
{
	public static AccessProtectedStorageWaitTime Default { get; } = new();

	AccessProtectedStorageWaitTime() : base(TimeSpan.FromSeconds(5)) {}
}