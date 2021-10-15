using AutoBogus;
using Bogus;
using DragonSpark.Compose;
using DragonSpark.Compose.Model.Commands;

namespace DragonSpark.Application.Entities.Generation;

sealed class Generator<T> : IGenerator<T> where T : class
{
	public static Generator<T> Default { get; } = new Generator<T>();

	Generator() : this(Configure<T>.Default.Then()) {}

	readonly CommandContext<IAutoGenerateConfigBuilder> _configure;

	public Generator(CommandContext<IAutoGenerateConfigBuilder> configure) => _configure = configure;

	public AutoFaker<T> Get(Configuration parameter)
	{
		var (seed, configure) = parameter;
		var result = new AutoFaker<T>().UseSeed(seed.GetValueOrDefault(Randomizer.Seed.Next().Grade()).Degrade())
		                               .To<AutoFaker<T>>()
		                               .Configure(_configure.Append(configure));
		return result;
	}
}