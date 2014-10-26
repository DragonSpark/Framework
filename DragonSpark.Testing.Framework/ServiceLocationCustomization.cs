using System.Reflection;
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
	public class ServiceLocationCustomization : ServiceLocatorImplBase, ICustomization, IServiceRegistry, IMethodCustomization
	{
		IFixture Fixture { get; set; }

		public void Customize( IFixture fixture )
		{
			Fixture = fixture;

			new[] { typeof(IServiceLocator), typeof(IServiceRegistry) }.Apply( x => Register( x, this ) );
		}

		protected override object DoGetInstance( Type serviceType, string key )
		{
			var result = new SpecimenContext( Fixture ).Resolve( new SeededRequest( serviceType, null ) );
			return result;
		}

		protected override IEnumerable<object> DoGetAllInstances( Type serviceType )
		{
			var result = new SpecimenContext( Fixture ).Resolve( new MultipleRequest( new SeededRequest( serviceType, null ) ) ).To<IEnumerable>().Cast<object>().ToArray();
			return result;
		}

		public void Register( Type @from, Type mappedTo )
		{
			this.GenericInvoke( "Register", new[] { from, mappedTo } );
		}

		void Register<TFrom, TTo>() where TTo : TFrom
		{
			Fixture.Customize<TFrom>( c => c.FromFactory( () => Fixture.Create<TTo>() ) );
		}

		public void Register( Type type, object instance )
		{
			this.GenericInvoke( "RegisterInstance", new[] { type }, instance );
		}

		void RegisterInstance<T>( T instance )
		{
			Fixture.Customize<T>( c => c.FromFactory( () => instance ).OmitAutoProperties() );
		}

		public void RegisterFactory( Type type, Func<object> factory )
		{
			this.GenericInvoke( "RegisterFactory", new[] { type }, factory );
		}

		void RegisterFactory<T>( Func<object> factory )
		{
			Fixture.Customize<T>( c => c.FromFactory( () => (T)factory() ).OmitAutoProperties() );
		}

		static T[] GetAll<T>( MemberInfo method ) where T : RegistrationAttribute
		{
			var type = method.DeclaringType;
			var result = type.Assembly.GetCustomAttributes<T>()
				.Concat( type.GetAttributes<T>() )
				.Concat( method.GetAttributes<T>() ).ToArray();
			return result;
		}

		public void Customizing( MethodInfo methodUnderTest, Type[] parameterTypes )
		{
			var register = GetAll<RegisterAttribute>( methodUnderTest );
			register.Apply( x => Register( x.From, x.To ) );

			GetAll<FreezeAttribute>( methodUnderTest ).Where( x => !x.After ).Apply( x => Register( x.From, GetInstance( x.To ) ) );
		}

		public void Customized( MethodInfo methodUnderTest, Type[] parameterTypes, IEnumerable<object[]> parameters )
		{
			GetAll<FreezeAttribute>( methodUnderTest ).Where( x => x.After ).Apply( x => Register( x.From, GetInstance( x.To ) ) );
		}
	}
}