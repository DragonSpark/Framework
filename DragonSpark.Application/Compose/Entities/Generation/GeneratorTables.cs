using Bogus;
using DragonSpark.Application.Entities.Generation;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using JetBrains.Annotations;
using System.Reflection;

namespace DragonSpark.Application.Compose.Entities.Generation
{
	sealed class GeneratorTables : ISelect<TypeInfo, IFakerTInternal>
	{
		public GeneratorTables(in uint? seed)
			: this(seed, Start.A.Generic(typeof(GeneratorTables<>)).Of.Type<ISelect<uint?, IFakerTInternal>>()) {}

		readonly uint?                                     _seed;
		readonly         IGeneric<ISelect<uint?, IFakerTInternal>> _generic;

		public GeneratorTables(in uint? seed, IGeneric<ISelect<uint?, IFakerTInternal>> generic)
		{
			_seed = seed;
			_generic   = generic;
		}

		public IFakerTInternal Get(TypeInfo parameter) => _generic.Get(parameter)().Get(_seed);
	}

	sealed class GeneratorTables<T> : Select<uint?, IFakerTInternal> where T : class
	{
		[UsedImplicitly]
		public static GeneratorTables<T> Default { get; } = new GeneratorTables<T>();

		GeneratorTables() : base(Generator<T>.Default) {}
	}
}