namespace DragonSpark.Composition.Compose;

sealed class Adapter : IRegistration
{
	readonly IRegistrationContext _registration;

	public Adapter(IRegistrationContext registration) => _registration = registration;

	public RegistrationResult Singleton() => _registration.Singleton().Result();

	public RegistrationResult Transient() => _registration.Transient().Result();

	public RegistrationResult Scoped() => _registration.Scoped().Result();
}