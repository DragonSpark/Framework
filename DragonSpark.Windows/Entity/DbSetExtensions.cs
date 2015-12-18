using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DragonSpark.Extensions;

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

	/*[AttributeUsage( AttributeTargets.Property  )]
	public class ApplyValueAttribute : Attribute, IAllowsPriority
	{
		public ApplyValueAttribute( Type type )
		{
			Type = type;
		}

		protected internal virtual void Each( object instance, PropertyInfo info, object current )
		{
			var provider = Activator.Create<IApplyValue>( Type, Parameters );
			provider.Each( instance, info, current );
		}

		protected virtual object[] Parameters => new object[0];

		public Type Type { get; }

		public virtual Priority Priority => Priority.Normal;
	}*/
}