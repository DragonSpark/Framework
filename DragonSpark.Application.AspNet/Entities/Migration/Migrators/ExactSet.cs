using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class ExactSet<T> : ISelect<DbSet<T>, IQueryable<T>> where T : class
{
	public static ExactSet<T> Default { get; } = new();

	ExactSet() {}

	public IQueryable<T> Get(DbSet<T> parameter)
	{
		var type = parameter.EntityType;
		return type.GetDiscriminatorPropertyName() is {} name && type.GetDiscriminatorValue() is {} value
			       ? parameter.Where(x => EF.Property<object>(x, name) == value)
			       : parameter;
	}
}