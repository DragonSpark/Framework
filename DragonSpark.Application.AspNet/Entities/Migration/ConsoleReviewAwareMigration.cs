using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class ConsoleReviewAwareMigration : IMigration
{
	readonly IMigration                 _previous;
	readonly IWorkspaces                _workspaces;
	readonly ICommand<EmitContextInput> _emit;

	public ConsoleReviewAwareMigration(IMigration previous, IWorkspaceDefinition workspaces)
		: this(previous, workspaces, EmitContext.Default) {}

	public ConsoleReviewAwareMigration(IMigration previous, IWorkspaceDefinition workspaces,
	                                   ICommand<EmitContextInput> emit)
	{
		_previous   = previous;
		_workspaces = workspaces;
		_emit       = emit;
	}

	public async ValueTask Get(Stop<ushort?> parameter)
	{
		var capture = Console.ForegroundColor;
		{
			await using var workspaces = _workspaces.Get();
			var (source, destination) = workspaces;

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(@"==================================================");
			Console.WriteLine(@" [REVIEW] DATA MIGRATION & STORAGE SETUP          ");
			Console.WriteLine(@"==================================================");

			_emit.Execute(new("Source Context", source));
			Console.WriteLine(@"--------------------------------------------------");
			_emit.Execute(new("Destination Context", destination));

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(@"==================================================");
		}

		Console.Write(@"[>] Continue migration against these targets? (y/n): ");

		var key = Console.ReadKey(true);
		Console.WriteLine();
		switch (key.Key)
		{
			case ConsoleKey.Enter:
			case ConsoleKey.Y:
				await _previous.Off(parameter);
				break;
			default:
				Console.ForegroundColor = capture;
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(@"[ABORT] Migration script aborted by user");
				Console.ForegroundColor = capture;
				Environment.Exit(1);
				return;
		}
	}
}

