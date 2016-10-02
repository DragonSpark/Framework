using DragonSpark.Application.Setup;
using System.Composition;

namespace DragonSpark.Testing.Objects.Setup
{
	[Export( typeof(ISetup) )]
	public partial class ProgramSetup
	{
		public ProgramSetup()
		{
			InitializeComponent();
		}
	}
}
