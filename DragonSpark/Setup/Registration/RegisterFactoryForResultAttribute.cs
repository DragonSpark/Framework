namespace DragonSpark.Setup.Registration
{
	public class RegisterFactoryForResultAttribute : RegistrationBaseAttribute
	{
		public RegisterFactoryForResultAttribute() : base( () => FactoryResultTypeRegistration.Instance )
		{}
	}
}