using DragonSpark.Application.Diagnostics;
using Microsoft.JSInterop;

namespace DragonSpark.Presentation.Environment;

public sealed class ShouldObserve : ProcessedExceptionCondition
{
	public static ShouldObserve Default { get; } = new();

	ShouldObserve()
		: base(x => x is JSDisconnectedException or JSException || x.StackTrace?.Contains("RemoteJSRuntime") == true) {}
}