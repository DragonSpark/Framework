using DragonSpark.Presentation.Environment.Browser;
using Microsoft.JSInterop;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Sequences.Virtualization;

sealed class ModuleReference : ModuleInstance, IJSObjectReference
{
	readonly IJSObjectReference _instance;

	public ModuleReference(IJSObjectReference module, IJSObjectReference instance) : base(module, instance, "dispose")
		=> _instance = instance;

	public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
		=> _instance.InvokeAsync<TValue>(identifier, args);

	public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken,
	                                             object?[]? args)
		=> _instance.InvokeAsync<TValue>(identifier, cancellationToken, args);
}