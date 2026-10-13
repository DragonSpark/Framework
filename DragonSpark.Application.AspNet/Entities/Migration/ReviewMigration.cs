using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class ReviewMigration : ICondition
{
	readonly bool                       _prompt;
	readonly IWorkspaceDefinition       _workspaces;
	readonly ICommand<EmitContextInput> _emit;

	public ReviewMigration(ReviewSettings settings, IWorkspaceDefinition workspaces)
		: this(settings.EnablePrompt, workspaces, EmitContext.Default) {}

	public ReviewMigration(bool prompt, IWorkspaceDefinition workspaces, ICommand<EmitContextInput> emit)
	{
		_prompt     = prompt;
		_workspaces = workspaces;
		_emit       = emit;
	}

	public bool Get(None parameter)
	{
		using var workspaces = _workspaces.Get();
		var (source, destination) = workspaces;
		var capture = Console.ForegroundColor;

		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine(@"==================================================");
		Console.WriteLine(@" [REVIEW] DATA MIGRATION & STORAGE INFORMATION    ");
		Console.WriteLine(@"==================================================");

		_emit.Execute(new("Source Context", source));
		Console.WriteLine(@"--------------------------------------------------");
		_emit.Execute(new("Destination Context", destination));

		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine(@"==================================================");

		if (_prompt)
		{
			Console.Write(@"[>] Continue migration against these targets? (y/n): ");

			var key = Console.ReadKey(true);
			Console.WriteLine();
			switch (key.Key)
			{
				case ConsoleKey.Enter:
				case ConsoleKey.Y:
					break;
				default:
					Console.ForegroundColor = capture;
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(@"[ABORT] Migration script aborted by user");
					Console.ForegroundColor = capture;
					Environment.Exit(1);
					return false;
			}
		}

		return true;
	}
}