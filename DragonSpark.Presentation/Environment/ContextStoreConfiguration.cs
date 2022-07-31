using DragonSpark.Application.Model;
using System;

namespace DragonSpark.Presentation.Environment;

sealed class ContextStoreConfiguration : RelativeExpiration
{
	public static ContextStoreConfiguration Default { get; } = new();

	ContextStoreConfiguration() : base(TimeSpan.FromSeconds(15)) {}
}