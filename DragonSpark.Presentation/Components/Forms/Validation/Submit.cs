using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation
{
	sealed class Submit : IOperation<EditContext>
	{
		readonly IOperation<EditContext>    _operation;
		readonly Operate<EditContext, bool> _valid;

		public Submit(IOperation<EditContext> operation) : this(operation, ValidContext.Default.Get) {}

		public Submit(IOperation<EditContext> operation, Operate<EditContext, bool> valid)
		{
			_operation = operation;
			_valid     = valid;
		}

		public async ValueTask Get(EditContext parameter)
		{
			if (await _valid(parameter))
			{
				await _operation.Get(parameter);
				parameter.MarkAsUnmodified();
			}
		}
	}
}