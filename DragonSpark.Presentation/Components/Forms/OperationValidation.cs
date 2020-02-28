using DragonSpark.Compose;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms
{
	public sealed class OperationValidation : IValidation // TODO: Remove.
	{
		public static OperationValidation Default { get; } = new OperationValidation();

		OperationValidation() : this("boo") {}

		readonly string _target;

		public OperationValidation(string target) => _target = target;

		public async ValueTask<bool> Get(FieldIdentifier parameter)
		{
			await Task.Delay(750);
			return parameter.Model.GetType()
			                .GetProperty(parameter.FieldName)
			                .GetValue(parameter.Model)
			                .To<string>() == _target;
		}
	}
}