using System;
using DragonSpark.Sources;

namespace DragonSpark.Testing.Framework
{
	public sealed class Output : Scope<Action<string>>
	{
		public static Output Default { get; } = new Output();
		Output() {}
	}
}