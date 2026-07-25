using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace DragonSpark.Application.AspNet.Entities.Search;

public sealed class FullTextSearchInterceptor : DbCommandInterceptor
{
	public static FullTextSearchInterceptor Default { get; } = new();

	FullTextSearchInterceptor() : this(RewriteCommand.Default) {}

	readonly ICommand<DbCommand> _rewrite;

	public FullTextSearchInterceptor(ICommand<DbCommand> rewrite) => _rewrite = rewrite;

	public override InterceptionResult<DbDataReader> ReaderExecuting(
		DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
	{
		_rewrite.Execute(command);
		return base.ReaderExecuting(command, eventData, result);
	}

	// ReSharper disable once TooManyArguments
	public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
		DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result,
		CancellationToken cancellationToken = default)
	{
		_rewrite.Execute(command);
		return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
	}
}