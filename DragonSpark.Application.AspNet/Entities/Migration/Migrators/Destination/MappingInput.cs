using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

public readonly record struct MappingInput<T>(DbContext Source, DbContext Destination, Array<T> Page, T From)
{
	public MappingInput(DbContext Source, DbContext Destination, T From)
		: this(Source, Destination, From.Yield().Result(), From) {}
}