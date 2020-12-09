using Bogus;
using DragonSpark.Application.Hosting.xUnit.Objects;
using DragonSpark.Compose;
using System;

namespace DragonSpark.Application.Hosting.xUnit
{
	public static class Extensions
	{
		public static T Generate<T>(this ISome<T> @this) where T : class
			=> @this.Generate(Randomizer.Seed.Next().Grade());

		public static Faker<T> Get<T>(this ISome<T> @this, IFakerTInternal other)
			where T : class => @this.Get(new Seeding<T>(Seedings.Default.Get(other)));

		public static Func<T> Bind<T>(this ISome<T> @this, IFakerTInternal other)
			where T : class => @this.Then().Select(x => x.Generate()).Bind(new Seeding<T>(Seedings.Default.Get(other)));

		public static T Generate<T>(this ISome<T> @this, uint seed) where T : class => @this.Get(new Seeding<T>(seed));
	}
}
