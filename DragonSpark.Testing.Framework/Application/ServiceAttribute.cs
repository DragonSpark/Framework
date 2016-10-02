using System;
using System.Reflection;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;

namespace DragonSpark.Testing.Framework.Application
{
	[AttributeUsage( AttributeTargets.Parameter )]
	public class ServiceAttribute : CustomizeAttribute
	{
		public override ICustomization GetCustomization( ParameterInfo parameter ) => new ServiceRegistration( parameter.ParameterType );
	}
}