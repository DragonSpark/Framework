using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation;

sealed class SubmittingAwareOperation : IOperation<EditContext>
{
	readonly IOperation<EditContext>      _previous;
	readonly ISelect<EditContext, Switch> _submitting;

	public SubmittingAwareOperation(IOperation<EditContext> valid, IOperation<EditContext> invalid) : this(new Submit(valid, invalid)) {}

	public SubmittingAwareOperation(IOperation<EditContext> previous) : this(previous, Submitting.Default) {}

	public SubmittingAwareOperation(IOperation<EditContext> previous, ISelect<EditContext, Switch> submitting)
	{
		_previous   = previous;
		_submitting = submitting;
	}

	public async ValueTask Get(EditContext parameter)
	{
		using var _ = _submitting.Get(parameter).Assigned(true);
		await _previous.Await(parameter);
	}
}