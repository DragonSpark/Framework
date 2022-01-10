using Bogus;
using DragonSpark.Application.Entities.Generation;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using JetBrains.Annotations;
using System.Reflection;

namespace DragonSpark.Application.Compose.Entities.Generation;

sealed class GeneratorTables : ISelect<TypeInfo, IFakerTInternal>
{
	public GeneratorTables(Application.Entities.Generation.Configuration configuration)
		: this(configuration, Start.A.Generic(typeof(GeneratorTables<>))
		                           .Of.Type<ISelect<Application.Entities.Generation.Configuration,
			                           IFakerTInternal>>()) {}

	readonly Application.Entities.Generation.Configuration                                     _configuration;
	readonly IGeneric<ISelect<Application.Entities.Generation.Configuration, IFakerTInternal>> _generic;

	public GeneratorTables(Application.Entities.Generation.Configuration configuration,
	                       IGeneric<ISelect<Application.Entities.Generation.Configuration, IFakerTInternal>> generic)
	{
		_configuration = configuration;
		_generic       = generic;
	}

	public IFakerTInternal Get(TypeInfo parameter) => _generic.Get(parameter)().Get(_configuration);
}

sealed class GeneratorTables<T> : Select<Application.Entities.Generation.Configuration, IFakerTInternal> where T : class
{
	[UsedImplicitly]
	public static GeneratorTables<T> Default { get; } = new GeneratorTables<T>();

	GeneratorTables() : base(Generator<T>.Default) {}
}