using System;
using System.Reflection;
using DragonSpark.Testing.Framework.Setup;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;

namespace DragonSpark.Testing.Framework.Parameters
{
	[AttributeUsage( AttributeTargets.Parameter )]
	public class LocatedAttribute : CustomizeAttribute
	{
		readonly bool enabled;

		public LocatedAttribute( bool enabled = true )
		{
			this.enabled = enabled;
		}

		public override ICustomization GetCustomization( ParameterInfo parameter ) => new ConfigureLocationCustomization( parameter.ParameterType, enabled );
	}
}