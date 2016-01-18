namespace DragonSpark.Windows.Testing.Setup
{
	public partial class UnitySetup
	{
		public class AutoDataAttribute : DragonSpark.Testing.Framework.Setup.AutoDataAttribute
		{
			public AutoDataAttribute() : base( DragonSpark.Testing.Objects.Setup.SetupFixtureFactory<UnitySetup>.Instance.Create )
			{ }
		}

		public UnitySetup()
		{
			InitializeComponent();
		}
	}
}
