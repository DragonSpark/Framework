using Microsoft.JSInterop;
using Polly;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class EvaluatePolicy : DragonSpark.Diagnostics.Policy
{
	public static EvaluatePolicy Default { get; } = new();

	EvaluatePolicy() : base(Policy.Handle<JSException>()) {}
}