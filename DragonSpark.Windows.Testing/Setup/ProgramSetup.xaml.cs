using DragonSpark.Testing.Framework.Setup;

namespace DragonSpark.Windows.Testing.Setup
{
	public partial class ProgramSetup
	{
		public class AutoDataAttribute : SetupAutoDataAttribute
		{
			public AutoDataAttribute() : base( DelegatedSetupAutoDataFactory<ProgramSetup>.Instance.Create )
			{ }
		}

		public ProgramSetup()
		{
			InitializeComponent();
		}
	}
}
