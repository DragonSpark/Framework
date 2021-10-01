using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.State;

namespace DragonSpark.Presentation.Compose
{
	public class OperationResultSelectorExtended<_, T> : Application.Compose.OperationResultSelectorExtended<_, T>
	{
		readonly ISelecting<_, T> _subject;

		public OperationResultSelectorExtended(ISelecting<_, T> subject) : base(subject) => _subject = subject;

		public OperationResultSelectorExtended<_, T> UpdateActivity(object receiver)
			=> new(new ActivityAwareSelecting<_, T>(_subject, receiver));
	}

}
