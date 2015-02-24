using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using DragonSpark.Activation;
using DragonSpark.Extensions;

namespace DragonSpark.Server.Legacy.Language.Expressions
{
	public abstract class ExpressionFactory<TSource, TParameter> : ExpressionFactory<object, TSource, TParameter>
	{
		protected override sealed Expression Create( ParameterExpression parameter, object source )
		{
			var result = Create( parameter );
			return result;
		}

		protected abstract Expression Create( ParameterExpression parameter );
	}

	public class FilterByPropertyExpressionFactory<TSource, TItem> : FilterExpressionFactory<TSource, TItem> where TSource : class
	{
		readonly IEnumerable<Tuple<string, string>> properties;

		public FilterByPropertyExpressionFactory( IEnumerable<Tuple<string, string>> properties )
		{
			this.properties = properties;
		}

		protected override Expression Create( ParameterExpression parameter, TSource source )
		{
			var first = properties.FirstOrDefault().Transform( x => CreateMethod( parameter, x, source ) );
			var result = properties.Skip( 1 ).Aggregate( first, ( current, name ) => Expression.And( current, CreateMethod( parameter, name, source ) ) );
			return result;
		}

		static Expression CreateMethod( Expression parameter, Tuple<string, string> tuple, object source )
		{
			var key = Objects.Synchronization.Expression.Evaluate( source, tuple.Item1 ).Last.Value;
			var property = Expression.Property( parameter, tuple.Item2 );
			var propertyType = typeof(TItem).GetProperty( tuple.Item2 ).PropertyType;
			var constantExpression = key.Value.Transform( x => Expression.Constant( x, propertyType ) ) ?? (Expression)Expression.Default( propertyType );
			var result = Expression.Equal( property, constantExpression );
			return result;
		}
	}

	public class FilterByTypeNameExpressionFactory<TType> : ExpressionFactory<TType, string>
	{
		static Expression Create( ParameterExpression parameter, Stack<Type> queue )
		{
			var type = queue.Pop();
			var result = Expression.Condition( Expression.TypeIs( parameter, type ), Expression.Constant( type.Name ), queue.Any() ? Create( parameter, queue ) : Expression.Constant( string.Empty ) );
			return result;
		}

		protected override Expression Create( ParameterExpression parameter )
		{
			var types = new Stack<Type>( typeof(TType).Prepend( typeof(TType).GetKnownTypes() ).Where( x => !x.IsAbstract && x.Namespace != "System.Data.Entity.DynamicProxies" ).ToArray() );
			var result = Create( parameter, types );
			return result;
		}
	}

	public abstract class ExpressionFactory<TSource, TParameter, TResult> : FactoryBase<TSource, Expression<Func<TParameter, TResult>>> where TSource : class
	{
		protected override sealed Expression<Func<TParameter, TResult>> CreateFrom( TSource source )
		{
			var parameter = Expression.Parameter( typeof(TParameter), "x" );
			var expression = Create( parameter, source );
			var result = Expression.Lambda<Func<TParameter, TResult>>( expression, parameter );
			return result;
		}

		protected abstract Expression Create( ParameterExpression parameter, TSource source );
	}

	public abstract class FilterExpressionFactory<TSource, TParameter> : ExpressionFactory<TSource, TParameter, bool> where TSource : class
	{}

	public abstract class FilterExpressionFactoryBase<TItem> : FactoryBase<string, Expression<Func<TItem, bool>>> where TItem : class
	{
		static readonly IDictionary<Type, IEnumerable<string>> Properties = new Dictionary<Type, IEnumerable<string>>();

		protected override Expression<Func<TItem, bool>> CreateFrom( string source )
		{
			if ( !string.IsNullOrEmpty( source ) )
			{
				var terms = source.ToStringArray( ' ' );
				var parameter = Expression.Parameter( typeof(TItem), "x" );
				var properties = Properties.Ensure( typeof(TItem), ResolveProperties ).ToArray();
				var first = properties.FirstOrDefault().Transform( x => CreateMethod( parameter, x, terms ) );
				var expression = properties.Skip( 1 ).Aggregate( first, ( current, name ) => Expression.Or( current, CreateMethod( parameter, name, terms ) ) );
				var result = Expression.Lambda<Func<TItem, bool>>( expression, parameter );
				return result;
			}
			return null;
		}

		protected abstract IEnumerable<string> ResolveProperties( Type arg );

		static Expression CreateMethod( Expression parameter, string name, IEnumerable<string> terms )
		{
			var property = Expression.Property( parameter, name );
			var notNull = Expression.NotEqual( property, Expression.Constant( null ) );
			var first = terms.FirstOrDefault().Transform( x => ResolveContains( property, x ) );
			var expression = terms.Skip( 1 ).Aggregate( first, ( current, term ) =>
			{
				var methodCallExpression = ResolveContains( property, term );
				var binaryExpression = Expression.Or( current, methodCallExpression );
				return binaryExpression;
			} );
			// var contains = System.Linq.Expressions.Expression.Call( property, "Contains", null, System.Linq.Expressions.Expression.Constant( filter ) );
			var result = Expression.And( notNull, expression );
			return result;
		}

		static Expression ResolveContains( MemberExpression property, string x )
		{
			var result = Expression.Call( property, "Contains", null, Expression.Constant( x ) );
			return result;
		}
	}

	public class SearchFilterExpressionFactory<TItem> : FilterExpressionFactoryBase<TItem> where TItem : class
	{
		public static SearchFilterExpressionFactory<TItem> Instance
		{
			get { return InstanceField; }
		}	static readonly SearchFilterExpressionFactory<TItem> InstanceField = new SearchFilterExpressionFactory<TItem>();

		protected override IEnumerable<string> ResolveProperties( Type arg )
		{
			var result = typeof(TItem).GetProperties().Where( x => x.PropertyType == typeof(string) && x.CanWrite && !x.IsDecoratedWith<NotMappedAttribute>() ).Select( x => x.Name ).ToArray();
			return result;
		}
	}
}
