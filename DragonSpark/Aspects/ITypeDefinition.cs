using DragonSpark.Aspects.Build;
using System.Collections.Generic;

namespace DragonSpark.Aspects
{
	public interface ITypeDefinition : ITypeAware, IEnumerable<IMethodStore> {}
}