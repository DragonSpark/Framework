using DragonSpark.Model.Sequences;
using System;
using Array = DragonSpark.Model.Sequences.Array;

namespace DragonSpark.Composition;

public readonly record struct ComponentRequest(Type Request, Type Result)
{
	public static implicit operator Array<Type>(ComponentRequest instance)
		=> Array.Of(instance.Request, instance.Result);

	public static implicit operator (Type, Type)(ComponentRequest instance) => (instance.Request, instance.Result);

	public static implicit operator Type(ComponentRequest instance) => instance.Result;
}