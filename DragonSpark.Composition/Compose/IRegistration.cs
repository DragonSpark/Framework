namespace DragonSpark.Composition.Compose
{
	public interface IRegistration
	{
		RegistrationResult Singleton();

		RegistrationResult Transient();

		RegistrationResult Scoped();
	}
}