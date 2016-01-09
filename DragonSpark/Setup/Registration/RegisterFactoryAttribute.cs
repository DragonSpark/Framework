namespace DragonSpark.Setup.Registration
{
	public class RegisterFactoryAttribute : RegistrationBaseAttribute
	{
		public RegisterFactoryAttribute() : base( () => FactoryRegistration.Instance )
		{}
	}
}