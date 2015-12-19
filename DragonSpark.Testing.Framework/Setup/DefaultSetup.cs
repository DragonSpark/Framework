using DragonSpark.Diagnostics;
using DragonSpark.Windows.Runtime;

namespace DragonSpark.Testing.Framework.Setup
{
	public partial class DefaultSetup
	{
		public DefaultSetup()
		{
			InitializeComponent();
		}
	}

	public class SetupApplicationCommand : SetupApplicationCommand<RecordingMessageLogger, AssemblyProvider>
	{}
}
