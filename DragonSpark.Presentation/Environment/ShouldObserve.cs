using DragonSpark.Model.Selection.Conditions;
using Microsoft.JSInterop;

namespace DragonSpark.Presentation.Environment;

sealed class ShouldObserve : ICondition<Exception>
{
	public static ShouldObserve Default { get; } = new();

	ShouldObserve() {}

	public bool Get(Exception parameter)
		=> parameter is JSDisconnectedException || parameter.StackTrace?.Contains("RemoteJSRuntime") == true;
}