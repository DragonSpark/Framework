using DragonSpark.Diagnostics;
using DragonSpark.Presentation.Environment.Browser.Document;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class ConnectionAwareStoreFocusedElement : ConnectionAware
{
	public ConnectionAwareStoreFocusedElement(StoreFocusedElement previous) : base(previous) {}
}

// TODO

sealed class PolicyAwareFocusedElement : PolicyAwareOperation
{
	public PolicyAwareFocusedElement(ConnectionAwareStoreFocusedElement previous)
		: base(previous, ConnectionAwarePolicy.Default) {}
}