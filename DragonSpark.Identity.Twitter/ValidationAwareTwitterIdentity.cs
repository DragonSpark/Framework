using DragonSpark.Application.Components.Validation.Expressions;
using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Identity.Twitter;

sealed class ValidationAwareTwitterIdentity : ITwitterIdentity
{
	readonly ITwitterIdentity       _previous;
	readonly IValidateValue<object> _validate;

	public ValidationAwareTwitterIdentity(ITwitterIdentity previous) : this(previous, HandleValidator.Default) {}

	public ValidationAwareTwitterIdentity(ITwitterIdentity previous, IValidateValue<object> validate)
	{
		_previous = previous;
		_validate = validate;
	}

	public ValueTask<string?> Get(string parameter)
		=> _validate.Get(parameter) ? _previous.Get(parameter) : default(string).ToOperation();
}