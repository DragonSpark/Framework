using DragonSpark.Model.Sequences;
using NetFabric.Hyperlinq;
using System;

namespace DragonSpark.Application.Entities.Queries.Compilation
{
	sealed class Candidates<TIn, TOut> : ArrayInstance<Generic<TIn, TOut>>
	{
		public static Candidates<TIn, TOut> Default { get; } = new Candidates<TIn, TOut>();

		Candidates() : this(typeof(Compiled<,,>), typeof(Compiled<,,,>), typeof(Compiled<,,,,>),
		                    typeof(Compiled<,,,,,>),
		                    typeof(Compiled<,,,,,,>), typeof(Compiled<,,,,,,,>), typeof(Compiled<,,,,,,,,>),
		                    typeof(Compiled<,,,,,,,,,>), typeof(Compiled<,,,,,,,,,,>)) {}

		public Candidates(params Type[] types)
			: base(types.AsValueEnumerable().Select(x => new Generic<TIn, TOut>(x)).ToArray()) {}
	}
}