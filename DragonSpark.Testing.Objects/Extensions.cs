using AutoBogus;
using AutoFixture;
using Bogus.Extensions;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Testing.Objects.Entities.Generation.Compose;
using System;
using System.Collections.Generic;
using Configuration = DragonSpark.Testing.Objects.Entities.Generation.Configuration;

namespace DragonSpark.Testing.Objects;

public static class Extensions
{
	public static ISelect<None, IEnumerable<T>> Many<T>(this IResult<IFixture> @this, uint count)
		=> @this.Then().Select(new Many<T>(count)).Accept().Get();

	public static GeneratorContext<T> Generator<T>(this ModelContext _, in uint? seed = null)
		where T : class => _.Generator<T>(new Configuration(seed));

	public static GeneratorContext<T> Generator<T>(this ModelContext _,
	                                               Action<IAutoGenerateConfigBuilder> configure)
		where T : class => _.Generator<T>(new Configuration(null, configure));

	public static GeneratorContext<T> Generator<T>(this ModelContext _, Configuration configuration)
		where T : class => new(configuration);

	public static IncludeMany<T, TOther> Between<T, TOther>(this IncludeMany<T, TOther> @this, Range range)
		where TOther : class
		=> @this.Generate((faker, _) => faker.GenerateBetween(range.Start.Value, range.End.Value));

	public static IncludeMany<T, TOther> Empty<T, TOther>(this IncludeMany<T, TOther> @this) where TOther : class
		=> @this.Generate((faker, _) => faker.Generate(0));
}