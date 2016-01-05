using DragonSpark.Testing.Framework.Setup;

namespace DragonSpark.Windows.Testing.Setup
{
	public partial class DefaultSetup
	{
		public class AutoDataAttribute : DragonSpark.Testing.Framework.Setup.AutoDataAttribute
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
