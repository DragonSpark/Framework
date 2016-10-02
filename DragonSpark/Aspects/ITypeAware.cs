using System;

namespace DragonSpark.Aspects
{
	public interface ITypeAware
	{
		Type DeclaringType { get; }
	}
}