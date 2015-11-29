using System.Collections.Generic;
using DragonSpark.Runtime;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework
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