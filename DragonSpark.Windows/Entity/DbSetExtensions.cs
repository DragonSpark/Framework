using DragonSpark.Extensions;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Windows.Entity
{
	public static class DbSetExtensions
	{
		public static TEntity Find<TEntity>( this IDbSet<TEntity> @this, Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IQueryable<TEntity>> with = null ) where TEntity : class
		{
			var result = @this.Local.FirstOrDefault( where.Compile() ) ?? with.With( x => x( @this ), () => @this ).FirstOrDefault( where );
			return result;
		}
	}
}