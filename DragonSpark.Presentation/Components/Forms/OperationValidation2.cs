using DragonSpark.Compose;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms
{
	public sealed class OperationValidation2 : IValidation // TODO: Remove.
	{
		public static OperationValidation2 Default { get; } = new OperationValidation2();

		OperationValidation2() {}

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