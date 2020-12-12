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
		public GeneratorTables(Configuration configuration)
			: this(configuration, Start.A.Generic(typeof(GeneratorTables<>))
			                           .Of.Type<ISelect<Configuration, IFakerTInternal>>()) {}

		readonly Configuration                                     _configuration;
		readonly IGeneric<ISelect<Configuration, IFakerTInternal>> _generic;

		public GeneratorTables(Configuration configuration, IGeneric<ISelect<Configuration, IFakerTInternal>> generic)
		{
			_configuration = configuration;
			_generic       = generic;
		}

		public IFakerTInternal Get(TypeInfo parameter) => _generic.Get(parameter)().Get(_configuration);
	}

	sealed class GeneratorTables<T> : Select<Configuration, IFakerTInternal> where T : class
	{
		[UsedImplicitly]
		public static GeneratorTables<T> Default { get; } = new GeneratorTables<T>();

		GeneratorTables() : base(Generator<T>.Default) {}
	}
}