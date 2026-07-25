using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Presentation.Environment.Browser;

public sealed class ModulePath : ConcurrentStore<Type, string>
{
	public static ModulePath Default { get; } = new();

	ModulePath() : base(ComposeModulePath.Default) {}
}