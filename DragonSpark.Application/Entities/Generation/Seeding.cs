using Bogus;
using DragonSpark.Compose;

namespace DragonSpark.Application.Entities.Generation
{
	// TODO: remove
	public readonly struct Seeding<T> where T : class
	{
		public Seeding(uint seed) : this(new Faker<T>().UseSeed(seed.Degrade()), seed) {}

		public Seeding(Faker<T> source, uint seed)
		{
			Source = source;
			Seed   = seed;
		}

		public Faker<T> Source { get; }

		public uint Seed { get; }

		public void Deconstruct(out Faker<T> source, out uint seed)
		{
			source = Source;
			seed   = Seed;
		}
	}

	/*public readonly struct Seeding<T, TOther> where T : class
	{
		public Faker<T> Source { get; }
		public uint Seed { get; }

		public Seeding(uint seed) : this(new Faker<T>().UseSeed(seed.Degrade()), seed) {}

		public Seeding(Faker<T> source, uint seed)
		{
			Source    = source;
			Seed = seed;
		}
	}*/
}