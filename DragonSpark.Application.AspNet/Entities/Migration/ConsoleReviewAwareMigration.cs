using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class ConsoleReviewAwareMigration : IMigration
{
	readonly IMigration  _previous;
	readonly IWorkspaces _workspaces;

	public ConsoleReviewAwareMigration(IMigration previous, IWorkspaceDefinition workspaces)
	{
		_previous   = previous;
		_workspaces = workspaces;
	}

	public async ValueTask Get(Stop<ushort> parameter)
	{
		var capture = Console.ForegroundColor;
		{
			await using var workspaces = _workspaces.Get();
			var (source, destination) = workspaces;
			Console.ForegroundColor   = ConsoleColor.Yellow;
			Console.WriteLine(@"==================================================");
			Console.WriteLine(@" REVIEW: DATA MIGRATION & STORAGE SETUP ");
			Console.WriteLine(@"==================================================");

			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(@" Source Context Type : ");
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine(source.GetType().Name);

			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(@" Source Connection   : ");
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine(source.Database.GetConnectionString().OrNone());

			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(@" Destination Context Type : ");
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine(destination.GetType().Name);

			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(@" Target Connection   : ");
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine(destination.Database.GetConnectionString().OrNone());

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(@"==================================================");
		}

		Console.Write(@"Continue migration against these targets? (y/n): ");

		var key = Console.ReadKey(true);
		Console.WriteLine();
		switch (key.Key)
		{
			case ConsoleKey.Y:
				await _previous.Off(parameter);
				break;
			default:
				Console.ForegroundColor = capture;
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(@"Migration script aborted by user");
				Console.ForegroundColor = capture;
				Environment.Exit(1);
				return;
		}
	}
}