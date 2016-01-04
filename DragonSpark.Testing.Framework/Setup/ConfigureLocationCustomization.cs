using DragonSpark.ComponentModel;
using DragonSpark.Testing.Framework.Setup.Location;
using Ploeh.AutoFixture;
using System;

namespace DragonSpark.Testing.Framework.Setup
{
	public class ConfigureLocationCustomization : CustomizationBase
	{
		readonly Type locationType;
		readonly bool enabled;

		public ConfigureLocationCustomization( Type locationType, bool enabled )
		{
			this.locationType = locationType;
			this.enabled = enabled;
		}

		[Activate]
		IServiceLocationAuthority Authority { get; set; }

		protected override void Customize( IFixture fixture )
		{
			Authority.Register( locationType, enabled );
		}
	}
}