using System;

namespace DragonSpark.Testing.TestObjects.IoC
{
	public interface INamedTypeObject
	{
		string Name { get; set; }
		Type Type { get; set; }
	}
}