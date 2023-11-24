using System;

namespace DragonSpark.Reflection;

sealed class Attribute<T> : AttributeStore<T> where T : Attribute
{
	public static Attribute<T> Default { get; } = new();

	Attribute() : base(Attributes<T>.Default) {}
}