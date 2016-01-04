using DragonSpark.Testing.Framework.Setup;

namespace DragonSpark.Windows.Testing.Setup
{
	public partial class DefaultSetup
	{
		public class AutoDataAttribute : SetupAutoDataAttribute
		{
			public AutoDataAttribute() : base( DelegatedSetupAutoDataFactory<DefaultSetup>.Instance.Create )
			{ }
		}

		public DefaultSetup()
		{
			InitializeComponent();
		}
	}
}
