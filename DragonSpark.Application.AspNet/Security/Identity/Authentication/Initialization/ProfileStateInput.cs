namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

sealed class ProfileStateInput<T> : IProfileStateInput where T : IdentityUser
{
	readonly AuthenticationStateValue<T> _previous;

	public ProfileStateInput(AuthenticationStateValue<T> previous) => _previous = previous;

	public CurrentProfileStateInput? Get()
		=> _previous.Get() is {} previous ? new(previous.User, previous.Profile) : null;
}