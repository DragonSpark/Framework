using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms.Validation;

sealed class SubmitWithCancel : IOperation<SubmittingInput>
{
	readonly IOperation<SubmittingInput> _valid, _invalid;
	readonly Operate<EditContext, bool>  _validate;

	public SubmitWithCancel(IOperation<SubmittingInput> valid, IOperation<SubmittingInput> invalid)
		: this(valid, invalid, ValidContext.Default.Get) {}

	public SubmitWithCancel(IOperation<SubmittingInput> valid, IOperation<SubmittingInput> invalid,
	                        Operate<EditContext, bool> validate)
	{
		_valid    = valid;
		_invalid  = invalid;
		_validate = validate;
	}

	public async ValueTask Get(SubmittingInput parameter)
	{
		var (context, cancel) = parameter;
		var valid = await _validate(context).On();
		if (valid)
		{
			await _valid.Get(parameter).On();
			context.MarkAsUnmodified();
		}
		else
		{
			cancel.Up();
			await _invalid.Off(parameter);
		}
	}
}