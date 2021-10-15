using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Commands;
using DragonSpark.Reflection.Assemblies;
using DragonSpark.Runtime.Environment;

namespace DragonSpark.Application.Diagnostics.Initialization;

class EmitProgramLog : Command
{
	protected EmitProgramLog(ILogMessage<string> message) : this(message, PrimaryAssemblyDetails.Default) {}

	protected EmitProgramLog(ILogMessage<string> message, AssemblyDetails details)
		: this(message, details.Title) {}

	protected EmitProgramLog(ILogMessage<string> message, string parameter)
		: base(message.Then().Bind(parameter)) {}
}