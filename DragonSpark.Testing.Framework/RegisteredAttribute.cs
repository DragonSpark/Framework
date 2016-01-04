using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
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
		class Customization : ICustomization
		{
			readonly ParameterInfo parameter;

			public Customization( ParameterInfo parameter )
			{
				this.parameter = parameter;
			}

			public void Customize( IFixture fixture ) => new FixtureRegistry( fixture ).RegisterFactory( parameter.ParameterType, Create );

			object Create()
			{
				var factoryType = FactoryReflectionSupport.Instance.GetFactoryType( parameter.ParameterType, parameter.Member.DeclaringType );
				var result = new FactoryBuiltObjectFactory().Create( new ObjectFactoryParameter( factoryType ) );
				return result;
			}
		}

		public override ICustomization GetCustomization( ParameterInfo parameter ) => new Customization( parameter );
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
				locator.Register( type, item );
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