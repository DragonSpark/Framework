using DragonSpark.Compose;
using DragonSpark.Reflection.Types;
using System;
using Array = DragonSpark.Model.Sequences.Array;

namespace DragonSpark.Model.Selection.Stores
{
	public sealed class ReferenceTables<TIn, TOut> : Select<Func<TIn, TOut>, ITable<TIn, TOut>> where TIn : class
	{
		public static ReferenceTables<TIn, TOut> Default { get; } = new ReferenceTables<TIn, TOut>();

		ReferenceTables() : this(IsValueType.Default.Get(A.Type<TOut>())
			                         ? typeof(StructureValueTable<,>)
			                         : typeof(ReferenceValueTable<,>)) {}

		public ReferenceTables(Type type) : base(Start.A.Generic(type)
		                                              .Of.Type<ITable<TIn, TOut>>()
		                                              .WithParameterOf<Func<TIn, TOut>>()
		                                              .Then()
		                                              .Bind(Array.Of(A.Type<TIn>(), A.Type<TOut>()))
		                                              .Get()
		                                              .Then()
		                                              .Assume()
		                                        ) {}
	}
}