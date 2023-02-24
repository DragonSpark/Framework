using DragonSpark.Diagnostics;
using Microsoft.JSInterop;

namespace DragonSpark.Presentation.Components.Content.Sequences.Virtualization;

sealed class VirtualizationPolicyBuilder : Builder<IJSObjectReference>
{
	public static VirtualizationPolicyBuilder Default { get; } = new();

	VirtualizationPolicyBuilder()
		: base(Polly.Policy<IJSObjectReference>.Handle<JSException>(HasArgument.Default.Get)) {}
}