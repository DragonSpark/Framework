using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using Microsoft.JSInterop;

namespace DragonSpark.Presentation.Components.Content.Sequences.Virtualization;

sealed class VirtualizationPolicy : Policy<IJSObjectReference>
{
	public static VirtualizationPolicy Default { get; } = new();

	VirtualizationPolicy()
		: base(VirtualizationPolicyBuilder.Default.Then().Select(IgnorePolicy<IJSObjectReference>.Default)) {}
}