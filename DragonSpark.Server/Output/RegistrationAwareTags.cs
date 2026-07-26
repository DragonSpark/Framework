using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Server.Output;

sealed class RegistrationAwareTags : ITags
{
	readonly ITags                _previous;
	readonly Array<IRegistration> _registrations;

	public RegistrationAwareTags(IRegistration registration) : this(registration.Yield()) {}

	public RegistrationAwareTags(IEnumerable<IRegistration> registrations)
		: this(Tags.Default, registrations) {}

	public RegistrationAwareTags(ITags previous, IEnumerable<IRegistration> registrations)
		: this(previous, registrations.Result()) {}

	public RegistrationAwareTags(ITags previous, Array<IRegistration> registrations)
	{
		_previous      = previous;
		_registrations = registrations;
	}

	public async ValueTask Get(Stop<ComposeTagsInput> parameter)
	{
		foreach (var registration in _registrations.Open())
		{
			await registration.Off(parameter);
		}

		await _previous.Off(parameter);
	}
}