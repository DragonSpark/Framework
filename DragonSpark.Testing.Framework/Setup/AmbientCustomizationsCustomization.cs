using System.Collections.Generic;
using System.Diagnostics;
using DragonSpark.Runtime.Values;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Setup
{
	public class AmbientCustomizationsCustomization : ICustomization
	{
		public static AmbientCustomizationsCustomization Instance { get; } = new AmbientCustomizationsCustomization();

		public void Customize( IFixture fixture )
		{
			AmbientValues.RegisterFor<IList<ICustomization>>( new List<ICustomization>(), fixture );
		}
	}
}