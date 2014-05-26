using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DragonSpark.Extensions;
using DragonSpark.Objects;
using DragonSpark.Objects.Synchronization;

namespace DragonSpark.Application.Communication.Entity
{
    public class FilterExpressionFactory<TSource,TItem> : Factory<TSource,Expression<Func<TItem, bool>>> where TSource : class
	{
		readonly IEnumerable<Tuple<string,string>> properties;

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		public FilterExpressionFactory( IEnumerable<Tuple<string,string>> properties )
		{
			this.properties = properties;
		}

		protected override Expression<Func<TItem,bool>> CreateItem( TSource source )
		{
			var parameter = System.Linq.Expressions.Expression.Parameter( typeof(TItem), "x" );
			var first = properties.FirstOrDefault().Transform( x => CreateMethod( parameter, x, source ) );
			var expression = properties.Skip( 1 ).Aggregate( first, ( current, name ) => System.Linq.Expressions.Expression.And( current, CreateMethod( parameter, name, source ) ) );
			var result = System.Linq.Expressions.Expression.Lambda<Func<TItem, bool>>( expression, parameter );
			return result;
		}

		static System.Linq.Expressions.Expression CreateMethod( System.Linq.Expressions.Expression parameter, Tuple<string,string> tuple, object source )
		{
			var key = source.Evaluate( tuple.Item1 ).Last.Value;
			var property = System.Linq.Expressions.Expression.Property( parameter, tuple.Item2 );
			var propertyType = typeof(TItem).GetProperty( tuple.Item2 ).PropertyType;
			var constantExpression = key.Value.Transform( x => System.Linq.Expressions.Expression.Constant( x, propertyType ) ) ?? (System.Linq.Expressions.Expression)System.Linq.Expressions.Expression.Default( propertyType );
			var result = System.Linq.Expressions.Expression.Equal( property, constantExpression );
			return result;
		}
	}
}