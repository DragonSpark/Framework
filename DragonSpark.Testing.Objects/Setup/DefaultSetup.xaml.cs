using DragonSpark.Testing.Framework.Setup;

namespace DragonSpark.Testing.Objects.Setup
{
	public partial class DefaultSetup
	{
		public class AutoDataAttribute : Framework.Setup.AutoDataAttribute
		{
			public AutoDataAttribute() : base( SetupFixtureFactory<DefaultSetup>.Instance.Create )
			{ }
		}

		public DefaultSetup()
		{
			InitializeComponent();
		}
	}
}
