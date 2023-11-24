using System;

namespace DragonSpark.Reflection;

sealed class Attributes<T> : AttributesStore<T> where T : Attribute
{
	public static Attributes<T> Default { get; } = new();

	Attributes() : base(IsDefined<T>.Inherited, ProvidedAttributes<T>.Inherited) {}
}