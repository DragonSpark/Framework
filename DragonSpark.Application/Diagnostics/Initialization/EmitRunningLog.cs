using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Reflection.Assemblies;
using DragonSpark.Runtime.Environment;
using System;
using System.Diagnostics;

namespace DragonSpark.Application.Diagnostics.Initialization;

public sealed class EmitRunningLog<T> : EmitRunningLog
{
	public static EmitRunningLog<T> Default { get; } = new EmitRunningLog<T>();

	EmitRunningLog() : base(LogRunningMessage<T>.Default) {}
}

public class EmitRunningLog : ICommand
{
	readonly ILogMessage<(string, Version, int)> _message;
	readonly AssemblyDetails                     _details;

	protected EmitRunningLog(ILogMessage<(string, Version, int)> message)
		: this(message, PrimaryAssemblyDetails.Default) {}

	protected EmitRunningLog(ILogMessage<(string, Version, int)> message, AssemblyDetails details)
	{
		_message = message;
		_details = details;
	}

	public void Execute(None parameter)
	{
		_message.Execute(_details.Title, _details.Version, Process.GetCurrentProcess().Id);
	}
}