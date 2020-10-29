using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation
{
	public interface IValidatingValue<in T> : IDepending<T> {}

	sealed class ValidatingValueAdapter<T> : IValidatingValue<T>
	{
		readonly IValidateValue<T> _validate;

		public ValidatingValueAdapter(IValidateValue<T> validate) => _validate = validate;

		public ValueTask<bool> Get(T parameter) => _validate.Get(parameter).ToOperation();
	}
}