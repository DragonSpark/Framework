using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	public interface IValidation : IOperationResult<FieldIdentifier, bool> {}

	public sealed class OperationValidation : IValidation
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

	public sealed class OperationValidation2 : IValidation
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