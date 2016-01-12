using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.Objects.Setup;

namespace DragonSpark.Windows.Testing.Setup
{
	public partial class LocationSetup
	{
		public class AutoDataAttribute : DragonSpark.Testing.Framework.Setup.AutoDataAttribute
		{
			public AutoDataAttribute() : base( SetupFixtureFactory<LocationSetup>.Instance.Create )
			{ }
		}

		public LocationSetup()
		{
			InitializeComponent();
		}
	}
}
