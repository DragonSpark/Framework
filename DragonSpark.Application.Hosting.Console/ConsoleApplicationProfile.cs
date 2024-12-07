using DragonSpark.Application.Compose.Undo;

namespace DragonSpark.Application.Hosting.Console;

sealed class ConsoleApplicationProfile : ApplicationProfile
{
	public static ConsoleApplicationProfile Default { get; } = new();

	ConsoleApplicationProfile() : base(_ => {}) {}
}