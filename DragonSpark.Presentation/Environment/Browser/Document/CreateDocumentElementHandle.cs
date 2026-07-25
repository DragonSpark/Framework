using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Presentation.Environment.Browser.Document;

sealed class CreateDocumentElementHandle : IStopAware<DocumentElement>
{
	readonly LoadModule<DocumentElement> _load;
	readonly NewDocumentElement          _new;

	public CreateDocumentElementHandle(LoadModule<DocumentElement> load) : this(load, NewDocumentElement.Default) {}

	public CreateDocumentElementHandle(LoadModule<DocumentElement> load, NewDocumentElement @new)
	{
		_load = load;
		_new  = @new;
	}

	public async ValueTask<DocumentElement> Get(CancellationToken parameter)
	{
		await using var load = new PolicyAwareJSObjectReference(await _load.Off(parameter));
		var @new = await _new.Off(new(load, parameter));
		return new(@new);
	}
}