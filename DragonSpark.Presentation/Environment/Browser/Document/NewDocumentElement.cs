namespace DragonSpark.Presentation.Environment.Browser.Document;

sealed class NewDocumentElement : CreateReference
{
	public static NewDocumentElement Default { get; } = new();

	NewDocumentElement() : base(nameof(NewDocumentElement)) {}
}