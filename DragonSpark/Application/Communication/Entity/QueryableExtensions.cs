using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Communication.Entity
{
	public static class QueryableExtensions
	{
		static readonly MethodInfo OfTypeTMethod = typeof(QueryableExtensions).GetMethod( "OfTypeT", DragonSparkBindingOptions.AllProperties );
		
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used to resolve expressions." )]
		public static IQueryable<TEntity> OfType<TEntity>( this IQueryable<TEntity> target, Type type, Expression<Func<TEntity,object>> selector = null ) where TEntity : class
		{
			var result = OfTypeTMethod.MakeGenericMethod( typeof(TEntity), type ).Invoke( null, new object[]{ target, selector } ).To<IQueryable<TEntity>>();
			return result;
		}

		static IQueryable<TEntity> OfTypeT<TEntity,TTarget>( IQueryable<TEntity> current, Expression<Func<TEntity,object>> selector = null )
		{
			var parameter = Expression.Parameter( typeof(TEntity), "x" );
			var item = selector.As<LambdaExpression>().Transform( x => ParameterRebinder.ReplaceParameters( new Dictionary<ParameterExpression, ParameterExpression>{ { x.Parameters.First(), parameter } },  x.Body ), () => parameter );
			var expression = Expression.Lambda<Func<TEntity, bool>>( Expression.TypeIs( item, typeof(TTarget) ), parameter );
			var result = current.Where( expression );
			return result;
		}

		public static IQueryable<TEntity> Query<TEntity>( this IQueryable<TEntity> target, string filter ) where TEntity : class
		{
			var result = SearchFilterExpressionFactory<TEntity>.Instance.Create( filter ).Transform( x => target.Where( x ), () => target );
			return result;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used to resolve expressions." )]
		public static Expression<T> Compose<T>( this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge )
		{
			var map = first.Parameters.Select( ( f, i ) => new { f, s = second.Parameters[ i ] } ).ToDictionary( p => p.s, p => p.f );
			var secondBody = ParameterRebinder.ReplaceParameters( map, second.Body );
			var result = Expression.Lambda<T>( merge( first.Body, secondBody ), first.Parameters );
			return result;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used to resolve expressions." )]
		public static Expression<Func<T, bool>> And<T>( this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second )
		{
			return first.Compose( second, Expression.And );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used to resolve expressions." )]
		public static Expression<Func<T, bool>> Or<T>( this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second )
		{
			return first.Compose( second, Expression.Or );
		}
	}
}