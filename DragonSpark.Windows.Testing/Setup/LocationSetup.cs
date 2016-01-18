namespace DragonSpark.Windows.Testing.Setup
{
	public partial class LocationSetup
	{
		public class AutoDataAttribute : DragonSpark.Testing.Framework.Setup.AutoDataAttribute
		{
			public AutoDataAttribute() : base( DragonSpark.Testing.Objects.Setup.SetupFixtureFactory<LocationSetup>.Instance.Create )
			{ }
		}

		public LocationSetup()
		{
			InitializeComponent();
		}
	}
}
