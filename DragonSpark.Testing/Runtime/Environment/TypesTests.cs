using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Execution;

// ReSharper disable All

namespace DragonSpark.Testing.Application.Runtime.Environment;

public sealed class TypesTests
{
	sealed class Types : Instances<Type>
	{
		public Types(SafeCounter counter, IEnumerable<Type> types) : base(types.Select(counter.Parameter!)) {}
	}
}