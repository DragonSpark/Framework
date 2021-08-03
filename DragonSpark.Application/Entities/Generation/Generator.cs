using AutoBogus;
using Bogus;
using DragonSpark.Compose;
using DragonSpark.Compose.Model.Commands;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Entities.Generation
{
	public interface IGenerator<T> : ISelect<Configuration, AutoFaker<T>> where T : class {}

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

	public readonly struct Configuration
	{
		public Configuration(in uint? seed) : this(in seed, _ => {}) {}

		public Configuration(in uint? seed, System.Action<IAutoGenerateConfigBuilder> configure)
		{
			Seed      = seed;
			Configure = configure;
		}

		public uint? Seed { get; }

		public System.Action<IAutoGenerateConfigBuilder> Configure { get; }

		public void Deconstruct(out uint? seed, out System.Action<IAutoGenerateConfigBuilder> auto)
		{
			seed = Seed;
			auto = Configure;
		}
	}
}