using System;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Types;

namespace DragonSpark.Model.Selection.Stores
{
	public sealed class ReferenceTables<TIn, TOut> : Select<Func<TIn, TOut>, ITable<TIn, TOut>> where TIn : class
	{
		public static ReferenceTables<TIn, TOut> Default { get; } = new ReferenceTables<TIn, TOut>();

		ReferenceTables() : this(IsValueType.Default.Get(typeof(TOut))
			                         ? typeof(StructureValueTable<,>)
			                         : typeof(ReferenceValueTable<,>)) {}

		public ReferenceTables(Type type) : base(Start.A.Generic(type)
		                                              .Of.Type<ITable<TIn, TOut>>()
		                                              .WithParameterOf<Func<TIn, TOut>>()
		                                              .In(new Array<Type>(typeof(TIn), typeof(TOut)))
		                                              .Assume()) {}
	}
}