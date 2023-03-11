using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Logging;
using System.IO;

namespace DragonSpark.Runtime;

public sealed class MessageAwareDeleteDirectoryRecursively : ICommand<DirectoryInfo>
{
	readonly ICommand<DirectoryInfo>  _previous;
	readonly DeletedDirectoryNotFound _log;

	public MessageAwareDeleteDirectoryRecursively(DeletedDirectoryNotFound log)
		: this(DeleteDirectoryRecursively.Default, log) {}

	public MessageAwareDeleteDirectoryRecursively(ICommand<DirectoryInfo> previous, DeletedDirectoryNotFound log)
	{
		_previous = previous;
		_log      = log;
	}

	public void Execute(DirectoryInfo parameter)
	{
		try
		{
			_previous.Execute(parameter);
		}
		catch (DirectoryNotFoundException exception)
		{
			_log.Execute(new(exception, parameter.FullName));
		}
	}

	public sealed class DeletedDirectoryNotFound : LogWarningException<string>
	{
		public DeletedDirectoryNotFound(ILogger<DeletedDirectoryNotFound> logger)
			: base(logger, "Attempt to delete {Location} but it was not found") {}
	}
}