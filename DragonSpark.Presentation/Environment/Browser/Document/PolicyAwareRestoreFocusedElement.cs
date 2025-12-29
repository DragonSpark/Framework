namespace DragonSpark.Presentation.Environment.Browser.Document;

sealed class PolicyAwareRestoreFocusedElement : ConnectionAwareOperation
{
	public PolicyAwareRestoreFocusedElement(RestoreFocusedElement previous) : base(previous) {}
}