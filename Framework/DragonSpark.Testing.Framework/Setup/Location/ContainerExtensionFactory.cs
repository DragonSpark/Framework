using System;
using DragonSpark.Activation;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Setup.Location
{
	public class ContainerExtensionFactory : RegisterFactoryAttribute
	{
		public ContainerExtensionFactory( Type registrationType ) : base( registrationType )
		{}

		protected override Func<object> GetFactory( IFixture fixture, IServiceRegistry registry )
		{
			Func<object> result = () => fixture.Create<IUnityContainer>().Extension( MappedTo );
			return result;
		}
	}
}