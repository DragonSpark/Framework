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
		public LocatedAttribute() : this( true )
		{}

		public LocatedAttribute( bool enabled )
		{
			Enabled = enabled;
		}

		public bool Enabled { get; }

		public override ICustomization GetCustomization( ParameterInfo parameter )
		{
			var result = new ConfigureLocationCustomization( parameter.ParameterType, Enabled );
			return result;
		}
	}
}