using System.Collections.Generic;

namespace DragonSpark.Diagnostics.Logging
{
	public interface IScalar : IReadOnlyDictionary<string, ScalarProperty> {}
}