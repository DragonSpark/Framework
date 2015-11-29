using System.Reflection;
using DragonSpark.Activation;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;

namespace DragonSpark.Testing.Framework
{
	public class FromCustomization : CustomizeAttribute
	{
		class Customization : ICustomization
		{
			public static Customization Instance { get; } = new Customization();

			public void Customize( IFixture fixture )
			{
				fixture.Freeze( Services.Location.Locator );
			}
		}

		public override ICustomization GetCustomization( ParameterInfo parameter )
		{
			var result = Customization.Instance;
			return result;
		}
	}
}