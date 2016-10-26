using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Application
{
	[AttributeUsage( AttributeTargets.Parameter )]
	public class ServiceAttribute : CustomizeAttribute
	{
		public override ICustomization GetCustomization( ParameterInfo parameter ) => new ServiceRegistrationCustomization( parameter.ParameterType );
	}
}