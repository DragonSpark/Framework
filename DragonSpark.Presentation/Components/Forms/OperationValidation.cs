using DragonSpark.Compose;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms
{
	public sealed class OperationValidation : IValidation // TODO: Remove.
	{
		public static OperationValidation Default { get; } = new OperationValidation();

		OperationValidation() {}

		public async ValueTask<bool> Get(FieldIdentifier parameter)
		{
			await Task.Delay(750);
			return parameter.Model.GetType()
			                .GetProperty(parameter.FieldName)
			                .GetValue(parameter.Model)
			                .To<string>() == "boo";
		}
	}
}