using DragonSpark.Application.Model.Interaction;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Compose;

public sealed class InteractionResultHandlerComposer<T> where T : IInteractionResult
{
	readonly IOperation<T> _subject;

	public InteractionResultHandlerComposer(IOperation<T> subject) => _subject = subject;

	public IInteractionResultHandler Adapt() => new ValidatingAwareResultHandler<T>(new Adapter<T>(_subject));
}