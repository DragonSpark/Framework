using DragonSpark.Model.Commands;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class EmitContext : ICommand<EmitContextInput>
{
	public static EmitContext Default { get; } = new();

	EmitContext() {}

	public void Execute(EmitContextInput parameter)
	{
		var (label, context) = parameter;

		var connection = context.Database.GetConnectionString();
		var builder    = new SqlConnectionStringBuilder(connection);

		var source = builder.DataSource;
		var local = source.Contains("(localdb)", StringComparison.OrdinalIgnoreCase) ||
		            source.Contains("localhost", StringComparison.OrdinalIgnoreCase) ||
		            source.Contains('.', StringComparison.OrdinalIgnoreCase) && source.Length <= 3;

		Console.ForegroundColor = ConsoleColor.DarkCyan;
		Console.WriteLine(@">> {0}:", label);

		Console.ForegroundColor = ConsoleColor.White;
		Console.Write(@"   Context Type : ");
		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine(context.GetType().Name);

		Console.ForegroundColor = ConsoleColor.White;
		Console.Write(@"   [Server]     : ");
		Console.ForegroundColor = local ? ConsoleColor.Green : ConsoleColor.Red;
		Console.WriteLine($@"{source} {(local ? "[LOCAL]" : "[EXTERNAL / CLOUD WARNING]")}");

		Console.ForegroundColor = ConsoleColor.White;
		Console.Write(@"   [Database]   : ");
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine(builder.InitialCatalog);

		var optionsBuilder = new SqlConnectionStringBuilder(connection)
		{
			DataSource     = string.Empty,
			InitialCatalog = string.Empty
		};

		Console.ForegroundColor = ConsoleColor.White;
		Console.Write(@"   [Options]    : ");
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(optionsBuilder.ConnectionString.TrimStart(';', ' '));
	}
}