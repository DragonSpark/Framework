using Polly;
using System;

namespace DragonSpark.Application.Entities.Diagnostics;

public sealed class ApplicationContentBuilder : DragonSpark.Diagnostics.Builder
{
	public static ApplicationContentBuilder Default { get; } = new ApplicationContentBuilder();

	ApplicationContentBuilder()
		: base(Policy.Handle<InvalidOperationException>(IsApplicationContentException.Default.Get)) {}
}