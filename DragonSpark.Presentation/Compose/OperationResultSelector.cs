using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.State;

namespace DragonSpark.Presentation.Compose;

public class OperationResultSelector<_, T> : Application.Compose.OperationResultSelector<_, T>
{
	readonly ISelecting<_, T> _subject;

	public OperationResultSelector(ISelecting<_, T> subject) : base(subject) => _subject = subject;

	public OperationResultSelector<_, T> UpdateActivity(object receiver)
		=> new(new ActivityAwareSelecting<_, T>(_subject, receiver));
}