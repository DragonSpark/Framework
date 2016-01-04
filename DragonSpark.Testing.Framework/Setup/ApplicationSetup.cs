using DragonSpark.Diagnostics;
using DragonSpark.TypeSystem;

namespace DragonSpark.Testing.Framework.Setup
{
	public class ApplicationSetup<TLogger, TAssemblyProvider> : DragonSpark.Setup.Commands.ApplicationSetup<TLogger, TAssemblyProvider, SetupAutoData> 
		where TLogger : IMessageLogger
		where TAssemblyProvider : IAssemblyProvider
	{}
}