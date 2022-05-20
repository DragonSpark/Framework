using DragonSpark.Presentation.Environment.Browser.Document;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class ConnectionAwareStoreFocusedElement : ConnectionAware
{
	public ConnectionAwareStoreFocusedElement(StoreFocusedElement previous) : base(previous) {}
}