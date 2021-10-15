using System;

namespace DragonSpark.Reflection;

sealed class LocalAttributes<T> : AttributesStore<T> where T : Attribute
{
	public static LocalAttributes<T> Default { get; } = new LocalAttributes<T>();

	LocalAttributes() : base(IsDefined<T>.Default, ProvidedAttributes<T>.Default) {}
}