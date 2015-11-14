using DragonSpark.Activation;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Testing.Framework
{
	public class ServiceLocator : ServiceLocatorImplBase
	{
		readonly SpecimenContext context;

		public ServiceLocator( IFixture fixture )
		{
			context = new SpecimenContext( fixture );

			var support = new FixtureSupport( fixture );
			support.RegisterInstance<IServiceLocator>( this );
			support.RegisterInstance<IServiceRegistry>( support );
			support.RegisterInstance( fixture );
		}

		protected override object DoGetInstance( Type serviceType, string key )
		{
			var result = context.Resolve( serviceType );
			return result;
		}

		protected override IEnumerable<object> DoGetAllInstances( Type serviceType )
		{
			var result = context.Resolve( new MultipleRequest( serviceType ) ).To<IEnumerable>().Cast<object>().ToArray();
			return result;
		}
	}

	class FixtureSupport : IServiceRegistry
	{
		readonly IFixture fixture;

		public FixtureSupport( IFixture fixture )
		{
			this.fixture = fixture;
		}

		public void Register( Type @from, Type mappedTo, string name = null )
		{
			fixture.Customizations.Add( new TypeRelay( @from, mappedTo ) );
		}

		public void Register( Type type, object instance )
		{
			this.InvokeGenericAction( "RegisterInstance", new[] { type }, instance );
		}

		public void RegisterInstance<T>( T instance )
		{
			fixture.Customize<T>( c => c.FromFactory( () => instance ).OmitAutoProperties() );
		}

		public void RegisterFactory( Type type, Func<object> factory )
		{
			this.InvokeGenericAction( "RegisterFactory", new[] { type }, factory );
		}

		public void RegisterFactory<T>( Func<object> factory )
		{
			fixture.Customize<T>( c => c.FromFactory( () => (T)factory() ).OmitAutoProperties() );
		}
	}
}