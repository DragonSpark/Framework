using DragonSpark.Diagnostics;

namespace DragonSpark.Presentation.Components.Content.Sequences.Virtualization;

sealed class HasArgument : HasAnyMessage
{
	public static HasArgument Default { get; } = new();

	HasArgument()
		: base("Argument 1 ('target') to IntersectionObserver.observe must be an instance of Element",
		       "IntersectionObserver.observe: Argument 1 is not an object",
		       "Failed to execute 'observe' on 'IntersectionObserver': parameter 1 is not of type 'Element'.") {}
}