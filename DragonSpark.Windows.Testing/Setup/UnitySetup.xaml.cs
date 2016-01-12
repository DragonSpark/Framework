using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.Objects.Setup;

namespace DragonSpark.Windows.Testing.Setup
{
	public partial class UnitySetup
	{
		public class AutoDataAttribute : DragonSpark.Testing.Framework.Setup.AutoDataAttribute
		{
			public AutoDataAttribute() : base( SetupFixtureFactory<UnitySetup>.Instance.Create )
			{ }
		}

		public UnitySetup()
		{
			InitializeComponent();
		}
	}
}
