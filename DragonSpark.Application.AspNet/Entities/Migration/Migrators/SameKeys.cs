using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class SameKeys<TFrom, TTo> : Result<bool> where TFrom : class where TTo : class
{
	public SameKeys(DbContext source, DbContext destination)
		: base(new ComposeSameKeys<TFrom, TTo>(source, destination).Then().Bind().Singleton()) {}
}