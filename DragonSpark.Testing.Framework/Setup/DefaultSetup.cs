using DragonSpark.Diagnostics;
using DragonSpark.Windows.Runtime;
using PostSharp.Patterns.Threading;

namespace DragonSpark.Testing.Framework.Setup
{
	[Synchronized]
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
