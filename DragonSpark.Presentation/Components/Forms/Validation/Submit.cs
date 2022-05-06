using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation;

sealed class Submit : IOperation<EditContext>
{
	readonly IOperation<EditContext>    _valid;
	readonly IOperation<EditContext>    _invalid;
	readonly Operate<EditContext, bool> _validate;

	public Submit(IOperation<EditContext> valid, IOperation<EditContext> invalid)
		: this(valid, invalid, ValidContext.Default.Get) {}

	public Submit(IOperation<EditContext> valid, IOperation<EditContext> invalid, Operate<EditContext, bool> validate)
	{
		_valid    = valid;
		_invalid  = invalid;
		_validate = validate;
	}

	public async ValueTask Get(EditContext parameter)
	{
		if (await _validate(parameter))
		{
			await _valid.Get(parameter);
			parameter.MarkAsUnmodified();
		}
		else
		{
			await _invalid.Await(parameter);
		}
	}
}