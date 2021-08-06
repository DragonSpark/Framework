using DragonSpark.Model.Sequences;
using System;
using Array = DragonSpark.Model.Sequences.Array;

namespace DragonSpark.Composition
{
	public readonly struct ComponentRequest
	{
		public static implicit operator Array<Type>(ComponentRequest instance)
			=> Array.Of(instance.Request, instance.Result);

		public static implicit operator (Type, Type)(ComponentRequest instance) => (instance.Request, instance.Result);

		public static implicit operator Type(ComponentRequest instance) => instance.Result;

		public ComponentRequest(Type request, Type result)
		{
			Request = request;
			Result  = result;
		}

		public Type Request { get; }

		public Type Result { get; }

		public void Deconstruct(out Type request, out Type result)
		{
			request = Request;
			result  = Result;
		}
	}
}
