using DragonSpark.Diagnostics;

namespace DragonSpark.Presentation.Environment.Browser.Document;

sealed class PolicyAwareRestoreFocusedElement : PolicyAwareOperation
{
	public PolicyAwareRestoreFocusedElement(ConnectionAwareRestoreFocusedElement previous)
		: base(previous, ConnectionAwarePolicy.Default) {}
}