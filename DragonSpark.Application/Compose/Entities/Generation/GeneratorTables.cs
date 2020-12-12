using Bogus;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using JetBrains.Annotations;
using System.Reflection;

namespace DragonSpark.Application.Compose.Entities.Generation
{
	sealed class GeneratorTables : ISelect<TypeInfo, IFakerTInternal>
	{
		public static GeneratorTables Default { get; } = new GeneratorTables();

		GeneratorTables() : this(Start.A.Generic(typeof(GeneratorTables<>)).Of.Type<IResult<IFakerTInternal>>()) {}

		readonly IGeneric<IResult<IFakerTInternal>> _generic;

		public GeneratorTables(IGeneric<IResult<IFakerTInternal>> generic) => _generic = generic;

		public IFakerTInternal Get(TypeInfo parameter) => _generic.Get(parameter)().Get();
	}

	sealed class GeneratorTables<T> : Result<IFakerTInternal> where T : class
	{
		[UsedImplicitly]
		public static GeneratorTables<T> Default { get; } = new GeneratorTables<T>();

		GeneratorTables() : base(Generator<T>.Default) {}
	}

}