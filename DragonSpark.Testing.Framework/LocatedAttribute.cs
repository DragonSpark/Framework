using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Reflection;

namespace DragonSpark.Testing.Framework
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

	public class ConfigureLocationCustomization : ICustomization
	{
		readonly Type locationType;
		readonly bool enabled;

		public ConfigureLocationCustomization( Type locationType, bool enabled )
		{
			this.locationType = locationType;
			this.enabled = enabled;
		}

		public void Customize( IFixture fixture )
		{
			fixture.Item<ServiceLocationCustomization>().Authority.Register( locationType, enabled );
		}
	}
}