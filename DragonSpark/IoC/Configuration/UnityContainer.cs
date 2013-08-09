
namespace DragonSpark.IoC.Configuration
{
	/*[ContentProperty( "Configurations" )]
	public class UnityContainer : Singleton<IUnityContainer>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "This is the factory method.  The result is disposed elsewhere." )]
		protected override IUnityContainer Create()
		{
			var result = new Microsoft.Practices.Unity.UnityContainer();
			Configurations.Apply( x => x.Configure( result ) );
			return result;
		}

		public Collection<IContainerConfigurationCommand> Configurations
		{
			get { return configurations; }
		}	readonly Collection<IContainerConfigurationCommand> configurations = new Collection<IContainerConfigurationCommand>();
	}*/
}
