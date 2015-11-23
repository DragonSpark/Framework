using System;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;

namespace DragonSpark.Testing.Framework
{
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
					fixture.TryCreate<object>( registrationType ).With( o => locator.Register( registrationType, o ) );
				} );
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