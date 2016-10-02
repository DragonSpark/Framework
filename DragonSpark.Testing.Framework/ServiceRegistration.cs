using System;
using DragonSpark.Activation.Location;
using DragonSpark.Application.Setup;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Runtime;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework
{
	public sealed class ServiceRegistration : ICustomization
	{
		readonly Type serviceType;

		public ServiceRegistration( Type serviceType )
		{
			this.serviceType = serviceType;
		}

		public void Customize( IFixture fixture )
		{
			var repository = AssociatedRegistry.Default.Get( fixture );
			var instance = GlobalServiceProvider.GetService<object>( serviceType );
			if ( instance.IsAssigned() )
			{
				repository.Add( new InstanceRegistrationRequest( serviceType, instance ) );
			}
		}
	}
}