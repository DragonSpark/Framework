using DragonSpark.Model.Selection.Stores;
using System;

namespace DragonSpark.Presentation.Environment.Browser;

public sealed class ModulePath : ConcurrentTable<Type, string>
{
	public static ModulePath Default { get; } = new();

	ModulePath() : base(ComposeModulePath.Default) {}
}