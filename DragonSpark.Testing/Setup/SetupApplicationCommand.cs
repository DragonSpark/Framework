using DragonSpark.Diagnostics;
using DragonSpark.Testing.Framework.Setup;
using AssemblyProvider = DragonSpark.Testing.TestObjects.AssemblyProvider;

namespace DragonSpark.Testing.Setup
{
	public class SetupApplicationCommand : SetupApplicationCommand<RecordingMessageLogger, AssemblyProvider>
	{}
}