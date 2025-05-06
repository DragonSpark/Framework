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

	public SubmittingAwareOperation(IOperation<EditContext> valid, IOperation<EditContext> invalid) :
		this(new Submit(valid, invalid)) {}

	public SubmittingAwareOperation(IOperation<EditContext> previous) : this(previous, Submitting.Default) {}

	public SubmittingAwareOperation(IOperation<EditContext> previous, ISelect<EditContext, Switch> submitting)
	{
		_previous   = previous;
		_submitting = submitting;
	}

	public async ValueTask Get(EditContext parameter)
	{
		using var _ = _submitting.Get(parameter).Assigned(true);
		await _previous.Off(parameter);
	}
}

sealed class SubmittingAwareOperation<T> : IOperation<SubmitInput<T>>
{
	readonly IOperation<SubmitInput<T>>   _previous;
	readonly ISelect<EditContext, Switch> _submitting;

	public SubmittingAwareOperation(IOperation<SubmitInput<T>> valid, IOperation<SubmitInput<T>> invalid)
		: this(new Submit<T>(valid, invalid)) {}

	public SubmittingAwareOperation(IOperation<SubmitInput<T>> previous) : this(previous, Submitting.Default) {}

	public SubmittingAwareOperation(IOperation<SubmitInput<T>> previous, ISelect<EditContext, Switch> submitting)
	{
		_previous   = previous;
		_submitting = submitting;
	}

	public async ValueTask Get(SubmitInput<T> parameter)
	{
		var (context, _) = parameter;
		using var _ = _submitting.Get(context).Assigned(true);
		await _previous.Off(parameter);
	}
}