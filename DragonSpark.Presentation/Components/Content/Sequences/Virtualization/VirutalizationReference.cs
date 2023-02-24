using DragonSpark.Presentation.Environment.Browser.Document;
using Microsoft.JSInterop;

namespace DragonSpark.Presentation.Components.Content.Sequences.Virtualization;

sealed class VirutalizationReference : PolicyAwareJSObjectReference
{
	public VirutalizationReference(IJSObjectReference previous) : base(previous, VirtualizationPolicy.Default.Get()) {}
}