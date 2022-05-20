using Microsoft.JSInterop;
using Polly;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class EvaluateBuilder : DragonSpark.Diagnostics.Builder
{
	public static EvaluateBuilder Default { get; } = new();

	EvaluateBuilder() : base(Policy.Handle<JSException>()) {}
}