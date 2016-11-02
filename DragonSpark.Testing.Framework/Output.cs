using DragonSpark.Sources.Scopes;
using System;

namespace DragonSpark.Testing.Framework
{
	public sealed class Output : Scope<Action<string>>
	{
		public static Output Default { get; } = new Output();
		Output() {}
	}
}