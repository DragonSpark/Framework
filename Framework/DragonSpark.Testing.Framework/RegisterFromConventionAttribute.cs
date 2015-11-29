using DragonSpark.Activation;
using DragonSpark.Setup;
using DragonSpark.Windows.Runtime;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework
{
	public class RegisterFromConventionAttribute : ICustomization
	{
		readonly IConventionRegistrationProfileProvider provider;
		readonly IFactory<IFixture, IConventionRegistrationService> factory;
		
		public RegisterFromConventionAttribute() : this( new ConventionRegistrationProfileProvider( AssemblyProvider.Instance ), UnityConventionRegistrationServiceFactory.Instance )
		{}

		public RegisterFromConventionAttribute( IConventionRegistrationProfileProvider provider, IFactory<IFixture, IConventionRegistrationService> factory )
		{
			this.provider = provider;
			this.factory = factory;
		}

		public void Customize( IFixture fixture )
		{
			var service = factory.Create( fixture );
			var profile = provider.Retrieve();
			service.Register( profile );
		}
	}
}