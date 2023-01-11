namespace DragonSpark.Presentation.Environment.Browser.Document;

sealed class ConnectionAwareRestoreFocusedElement : ConnectionAware
{
	public ConnectionAwareRestoreFocusedElement(RestoreFocusedElement previous) : base(previous) {}
}