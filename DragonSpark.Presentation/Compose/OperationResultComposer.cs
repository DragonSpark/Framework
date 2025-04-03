using DragonSpark.Model.Operations.Selection;
using DragonSpark.Presentation.Components.State;

namespace DragonSpark.Presentation.Compose;

public class OperationResultComposer<_, T> : Application.Compose.OperationResultComposer<_, T>
{
	readonly ISelecting<_, T> _subject;

	public OperationResultComposer(ISelecting<_, T> subject) : base(subject) => _subject = subject;

	public OperationResultComposer<_, T> UpdateActivity(IActivityReceiver receiver)
		=> new(new ActivityAwareSelecting<_, T>(_subject, receiver));

	public OperationResultComposer<_, T> UpdateActivityWithPostRedraw(IActivityReceiver receiver)
		=> new(new ActivityAwareSelecting<_, T>(_subject, receiver, UpdateActivityReceiverWithRedraw.Default));
}