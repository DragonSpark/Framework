using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using DragonSpark.Extensions;
using DragonSpark.Setup.Registration;
using DragonSpark.Testing.Framework.Setup.Location;
using Microsoft.Practices.ServiceLocation;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.Xunit2;
using PostSharp.Patterns.Contracts;
using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Testing.Framework
{
	public class FactoryAttribute : CustomizeAttribute
	{
		readonly Type factoryType;

		public FactoryAttribute( Type factoryType = null )
		{
			this.factoryType = factoryType;
		}

		public override ICustomization GetCustomization( ParameterInfo parameter )
		{
			var type = factoryType ?? ParameterInfoFactoryTypeLocator.Instance.Create( parameter );
			var registration = new FactoryRegistration( type, parameter.ParameterType /*, parameter.ParameterType, Factory.GetResultType( type ), parameter.ParameterType*/ );
			var result = new RegistrationCustomization( registration );
			return result;
		}
	}

	public static class FixtureExtensions
	{
		public static T Create<T>( this IFixture @this, Type type ) => (T)new SpecimenContext( @this ).Resolve( type );
	}

	public class RegisteredAttribute : CustomizeAttribute
	{
		class Customization : CustomizationBase
		{
			readonly Type serviceLocatorType;
			readonly Type registrationType;

			public Customization( [Required, OfType( typeof(IServiceLocator) )]Type serviceLocatorType, [Required]Type registrationType )
			{
				this.serviceLocatorType = serviceLocatorType;
				this.registrationType = registrationType;
			}

			protected override void Customize( IFixture fixture )
			{
				new FreezingCustomization( registrationType ).Customize( fixture );

				var locator = fixture.Create<IServiceLocator>( serviceLocatorType );
				var instance = fixture.Create<object>( registrationType );
				var item = instance.AsTo<Mock, object>( mock => mock.Object ) ?? instance;
				var type = instance is Mock ? registrationType.Adapt().GetInnerType() : registrationType;
				var registry = locator.GetInstance<IServiceRegistry>();
				registry.Register( type, item );
			}
		}

		public override ICustomization GetCustomization( ParameterInfo parameter )
		{
			var serviceLocatorType = parameter.Member.AsTo<MethodInfo, Type>( x => x.GetParameters().Select( info => info.ParameterType ).FirstOrDefault( typeof(IServiceLocator).IsAssignableFrom ) ) ?? typeof(IServiceLocator);
			var result = new Customization( serviceLocatorType, parameter.ParameterType );
			return result;
		}
	}
}