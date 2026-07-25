using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms.Validation;

sealed class SubmitWithCancelOperation : IOperation<SubmittingInput>
{
	readonly IOperation<SubmittingInput>  _previous;
	readonly ISelect<EditContext, Switch> _submitting;

	public SubmitWithCancelOperation(IOperation<SubmittingInput> valid, IOperation<SubmittingInput> invalid)
		: this(new SubmitWithCancel(valid, invalid)) {}

	public SubmitWithCancelOperation(IOperation<SubmittingInput> previous) : this(previous, Submitting.Default) {}

	public SubmitWithCancelOperation(IOperation<SubmittingInput> previous, ISelect<EditContext, Switch> submitting)
	{
		_previous   = previous;
		_submitting = submitting;
	}

	public async ValueTask Get(SubmittingInput parameter)
	{
		var (context, _) = parameter;
		using var _ = _submitting.Get(context).Assigned(true);
		await _previous.Off(parameter);
	}
}