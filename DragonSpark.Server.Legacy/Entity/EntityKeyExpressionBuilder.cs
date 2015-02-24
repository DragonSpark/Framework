using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DragonSpark.Activation;
using DragonSpark.Extensions;

namespace DragonSpark.Server.Legacy.Entity
{
	public class EntityKeyExpressionBuilder<TItem> : FactoryBase<Expression<Func<TItem, bool>>>
	{
		readonly IDictionary<string, object> key;

		public EntityKeyExpressionBuilder( IDictionary<string, object> key )
		{
			this.key = key;
		}

		protected override Expression<Func<TItem, bool>> CreateFrom( object source )
		{
			var parameter = Expression.Parameter( typeof(TItem), "x" );
			var first = key.FirstOrDefault().Transform( x => CreateMethod( parameter, x.Key, x.Value ) );
			var expression = key.Skip( 1 ).Aggregate( first, ( current, item ) => Expression.And( current, CreateMethod( parameter, item.Key, item.Value ) ) );
			var result = Expression.Lambda<Func<TItem, bool>>( expression, parameter );
			return result;
		}

		static Expression CreateMethod( Expression parameter, string propertyName, object value )
		{
			var property = Expression.Property( parameter, propertyName );
			var propertyType = typeof(TItem).GetProperty( propertyName ).PropertyType;
			var constantExpression = value.Transform( x => Expression.Constant( x, propertyType ) ) ?? (Expression)Expression.Default( propertyType );
			var result = Expression.Equal( property, constantExpression );
			return result;
		}
	}
}