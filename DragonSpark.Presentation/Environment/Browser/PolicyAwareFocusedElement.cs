using DragonSpark.Presentation.Environment.Browser.Document;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class PolicyAwareFocusedElement : ConnectionAwareOperation
{
	public PolicyAwareFocusedElement(StoreFocusedElement previous) : base(previous) {}
}