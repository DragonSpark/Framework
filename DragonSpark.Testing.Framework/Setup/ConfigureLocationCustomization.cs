using System;
using DragonSpark.Testing.Framework.Extensions;
using DragonSpark.Testing.Framework.Setup.Location;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Setup
{
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