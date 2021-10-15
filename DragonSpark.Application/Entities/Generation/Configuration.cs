using AutoBogus;

namespace DragonSpark.Application.Entities.Generation;

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