using System;

namespace DragonSpark.Reflection;

sealed class LocalAttribute<T> : AttributeStore<T> where T : Attribute
{
	public static LocalAttribute<T> Default { get; } = new();

	LocalAttribute() : base(LocalAttributes<T>.Default) {}
}