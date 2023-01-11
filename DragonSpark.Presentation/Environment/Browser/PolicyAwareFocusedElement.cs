using DragonSpark.Diagnostics;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class PolicyAwareFocusedElement : PolicyAwareOperation
{
	public PolicyAwareFocusedElement(ConnectionAwareStoreFocusedElement previous)
		: base(previous, ConnectionAwarePolicy.Default) {}
}