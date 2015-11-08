using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Activation;
using DragonSpark.Extensions;

namespace DragonSpark.Testing.Framework
{
	public class ServiceLocator : ServiceLocatorImplBase
	{
		readonly SpecimenContext context;

		/*public ServiceLocator() : this( new Fixture() )
		{}*/

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
			this.InvokeGenericAction( "Register", new[] { from, mappedTo } );
		}

		public void Register<TFrom, TTo>() where TTo : TFrom
		{
			fixture.Customize<TFrom>( c => c.FromFactory( () => fixture.Create<TTo>() ) );
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

	/*interface IMethodAware
	{
		void Customize( MethodInfo methodUnderTest );
	}

	public class ApplyRegistrationsCustomization : IMethodAware
	{
		

		/*public void Customizing( MethodInfo methodUnderTest )
		{
			/*var register = GetAll<RegisterAttribute>( methodUnderTest );
			register.Apply( x => Register( x.From, x.To ) );#2#

			Apply( methodUnderTest, x => !x.After );
		}

		public void Customized( MethodInfo methodUnderTest, Type[] parameterTypes, IEnumerable<object[]> parameters )
		{
			Apply( methodUnderTest, x => x.After );
		}#1#

		void Apply( MemberInfo methodUnderTest, Func<FreezeAttribute, bool> where )
		{
			/*GetAll<FreezeAttribute>( methodUnderTest )
				.Where( where )
				.Apply( x => Register( x.From, x.GetInstance( this, x.To ) ) );#1#
		}

		/*public void Customize( IFixture fixture )
		{
			// var info = fixture.Create<MemberInfo>();

			/*var register = GetAll<RegisterAttribute>( methodUnderTest );
			register.Apply( x => Register( x.From, x.To ) );#2#
		}#1#

		public void Customize( MethodInfo methodUnderTest )
		{
		}
	}*/
}