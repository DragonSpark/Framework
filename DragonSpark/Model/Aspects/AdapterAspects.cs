using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Types;
using System;
using System.Linq;
using Array = DragonSpark.Model.Sequences.Array;

namespace DragonSpark.Model.Aspects
{
	sealed class AdapterAspects<TIn, TOut> : ISelect<object, IAspect<TIn, TOut>>
	{
		readonly static Array<Type> Types = Array.Of(A.Type<TIn>(), A.Type<TOut>());

		public static AdapterAspects<TIn, TOut> Default { get; } = new AdapterAspects<TIn, TOut>();

		AdapterAspects() : this(Implementations.Arguments, Start.A.Generic(typeof(Cast<,,,>))
		                                                        .Of.Type<IAspect<TIn, TOut>>()
		                                                        .WithParameterOf<object>()) {}

		readonly Func<object, Array<Type>>            _arguments;
		readonly IGeneric<object, IAspect<TIn, TOut>> _generic;

		public AdapterAspects(Func<object, Array<Type>> arguments, IGeneric<object, IAspect<TIn, TOut>> generic)
		{
			_arguments = arguments;
			_generic   = generic;
		}

		public IAspect<TIn, TOut> Get(object parameter)
		{
			var types  = _arguments(parameter).Open().Append(Types.Open()).ToArray();
			var result = _generic.Get(types)(parameter);
			return result;
		}
	}
}