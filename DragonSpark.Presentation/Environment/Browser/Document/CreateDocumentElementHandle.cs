using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Environment.Browser.Document;

sealed class CreateDocumentElementHandle : Resulting<DocumentElement>
{
	public CreateDocumentElementHandle(LoadModule<DocumentElement> load) : this(load, NewDocumentElement.Default) {}

	public CreateDocumentElementHandle(LoadModule<DocumentElement> load, NewDocumentElement @new)
		: base(load.Then()
		           .Select(x => new PolicyAwareJSObjectReference(x))
		           .Select(@new)
		           .Select(x => new DocumentElement(x))) {}
}