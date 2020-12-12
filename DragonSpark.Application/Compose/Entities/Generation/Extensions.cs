using Bogus.Extensions;
using DragonSpark.Compose;
using System;

namespace DragonSpark.Application.Compose.Entities.Generation
{
	public static class Extensions
	{
		public static GeneratorContext<T> Generator<T>(this ModelContext _)
			where T : class => new GeneratorContext<T>();

		public static IncludeMany<T, TOther> Between<T, TOther>(this IncludeMany<T, TOther> @this, Range range) where TOther : class
			=> @this.Generate((faker, arg2) => faker.GenerateBetween(range.Start.Value, range.End.Value));
		public static IncludeMany<T, TOther> Empty<T, TOther>(this IncludeMany<T, TOther> @this) where TOther : class
			=> @this.Generate((faker, arg2) => faker.Generate(0));
	}
}