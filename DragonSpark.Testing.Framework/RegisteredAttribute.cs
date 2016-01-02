using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Extensions;
using DragonSpark.Testing.Framework.Setup.Location;
using Microsoft.Practices.ServiceLocation;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
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

			public void Customize( IFixture fixture )
			{
				new FixtureRegistry( fixture ).RegisterFactory( parameter.ParameterType, Create );
			}

			object Create()
			{
				var factoryType = FactoryReflectionSupport.Instance.GetFactoryType( parameter.ParameterType, parameter.Member.DeclaringType );
				var result = new FactoryBuiltObjectFactory().Create( new ObjectFactoryParameter( factoryType ) );
				return result;
			}
		}

		public override ICustomization GetCustomization( ParameterInfo parameter )
		{
			return new Customization( parameter );
		}
	}

	public class RegisteredAttribute : CustomizeAttribute
	{
		class Customization : ICustomization
		{
			readonly Type serviceLocationType;
			readonly Type registrationType;

			public Customization( Type serviceLocationType, Type registrationType )
			{
				this.serviceLocationType = serviceLocationType;
				this.registrationType = registrationType;
			}

			public void Customize( IFixture fixture )
			{
				fixture.TryCreate<IServiceLocator>( serviceLocationType ).With( locator =>
				{
					fixture.TryCreate<object>( registrationType ).With( x => Register( locator, x ) );
				} );
			}

			void Register( IServiceLocator locator, object instance )
			{
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