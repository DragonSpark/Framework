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

		GeneratorTables() : this(new Generic<IResult<IFakerTInternal>>(typeof(Tables<>))) {}

		readonly IGeneric<IResult<IFakerTInternal>> _generic;

		public GeneratorTables(IGeneric<IResult<IFakerTInternal>> generic) => _generic = generic;

		public IFakerTInternal Get(TypeInfo parameter) => _generic.Get(parameter)().Get();

		sealed class Tables<T> : IResult<IFakerTInternal> where T : class
		{
			[UsedImplicitly]
			public static Tables<T> Instance { get; } = new Tables<T>();

			Tables() {}

			public IFakerTInternal Get() => Generator<T>.Default.Get();
		}
	}
}