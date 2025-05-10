namespace DragonSpark.Presentation.Environment.Browser.Document;

sealed class PolicyAwareRestoreFocusedElement : ConnectionAware
{
	public PolicyAwareRestoreFocusedElement(RestoreFocusedElement previous) : base(previous) {}
}