using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Polly;
using System.Threading.Tasks;
using Policy = Polly.Policy;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IEvaluate>().Forward<Evaluate>().Decorate<PolicyAwareEvaluate>().Scoped();
	}
}

public interface IEvaluate : IOperation<string> {}

sealed class Evaluate : IEvaluate
{
	readonly IJSRuntime _runtime;

	public Evaluate(IJSRuntime runtime) => _runtime = runtime;

	public ValueTask Get(string parameter)
		=> !string.IsNullOrWhiteSpace(parameter)
			   ? _runtime.InvokeVoidAsync("eval", parameter)
			   : ValueTask.CompletedTask;
}

sealed class EvaluatePolicy : DragonSpark.Diagnostics.Policy
{
	public static EvaluatePolicy Default { get; } = new();

	EvaluatePolicy() : base(Policy.Handle<JSException>()) {}
}

sealed class PolicyAwareEvaluate : PolicyAwareOperation<string>, IEvaluate
{
	public PolicyAwareEvaluate(IEvaluate previous) : base(previous, DurableEvaluatePolicy.Default.Get()) {}
}

public sealed class DurableEvaluatePolicy : DeferredSingleton<IAsyncPolicy>
{
	public static DurableEvaluatePolicy Default { get; } = new();

	DurableEvaluatePolicy()
		: base(EvaluatePolicy.Default.Then().Select(DefaultRetryPolicy.Default)) {}
}