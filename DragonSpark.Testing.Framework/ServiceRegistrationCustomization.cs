using DragonSpark.Application.Setup;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Testing.Framework.Runtime;
using JetBrains.Annotations;
using Ploeh.AutoFixture;
using System;
using Defaults = DragonSpark.Activation.Location.Defaults;

namespace DragonSpark.Testing.Framework
{
	public sealed class ServiceRegistrationCustomization : SuppliedSource<Type, object>, ICustomization
	{
		readonly static Func<IFixture, IServiceRepository> RepositorySource = AssociatedRegistry.Default.Get;

		readonly Func<IFixture, IServiceRepository> repositorySource;
		readonly Type serviceType;

		public ServiceRegistrationCustomization( Type serviceType ) : this( RepositorySource, Defaults.ServiceSource, serviceType ) {}

		[UsedImplicitly]
		public ServiceRegistrationCustomization( Func<IFixture, IServiceRepository> repositorySource, Func<Type, object> source, Type serviceType ) : base( source, serviceType )
		{
			this.repositorySource = repositorySource;
			this.serviceType = serviceType;
		}

		public void Customize( IFixture fixture )
		{
			var instance = Get();
			if ( instance.IsAssigned() )
			{
				var registry = repositorySource( fixture );
				registry.Add( new ServiceRegistration( serviceType, instance ) );
			}
			else
			{
				throw new InvalidOperationException( $"{GetType().FullName} could not locate {serviceType}." );
			}
		}
	}
}