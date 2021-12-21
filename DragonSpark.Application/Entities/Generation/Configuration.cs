using AutoBogus;

namespace DragonSpark.Application.Entities.Generation;

public readonly record struct Configuration(in uint? Seed, System.Action<IAutoGenerateConfigBuilder> Configure)
{
	public Configuration(in uint? seed) : this(in seed, _ => {}) {}
}