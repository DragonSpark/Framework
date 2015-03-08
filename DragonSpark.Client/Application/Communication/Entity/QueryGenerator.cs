using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Client;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Communication.Entity
{
	public class QueryGenerator : IQueryGenerator
	{
		static readonly MethodInfo GenerateMethod = typeof(QueryGenerator).GetMethod( "GenerateInternal", DragonSparkBindingOptions.AllProperties );

		public static QueryGenerator Instance
		{
			get { return InstanceField; }
		}	static readonly QueryGenerator InstanceField = new QueryGenerator();

		protected virtual object[] ResolveParameters( MethodInfo methodInfo, IEnumerable<object> parameters  )
		{
			var items = new Stack<object>( parameters ?? Enumerable.Empty<object>() );
			var result = methodInfo.GetParameters().Select( x => items.Any() ? items.Pop() : x.ParameterType.GetDefaultValue() ).ToArray();
			return result;
		}

		protected virtual MethodInfo ResolveMethodInfo<TEntity>( Type type, string methodName = null ) where TEntity : System.ServiceModel.DomainServices.Client.Entity
		{
			var result = methodName.Transform( x => type.GetMethod( x ) ?? type.GetMethod( string.Concat( x, "Query" ) ) ) ?? type.GetMethods().FirstOrDefault( x => x.ReturnType == typeof(EntityQuery<TEntity>) );
			return result;
		}

		protected virtual EntityQuery<TEntity> GenerateInternal<TEntity>( DomainContext context, string methodName, IEnumerable<object> parameters, IEnumerable<IQueryShaper> shapers ) where TEntity : System.ServiceModel.DomainServices.Client.Entity
		{
			var type = context.GetType();
			var methodInfo = ResolveMethodInfo<TEntity>( type, methodName );
			var instances = ResolveParameters( methodInfo, parameters );
			var invoke = methodInfo.Invoke( context, instances );
			var query = invoke.To<EntityQuery<TEntity>>();
			var result = shapers.Aggregate( query, ( a, b ) => b.Shape( a ) );
			return result;
		}

		public EntityQuery Generate( Type entityType, DomainContext context, string methodName, IEnumerable<object> parameters, params IQueryShaper[] shapers )
		{
			var result = GenerateMethod.MakeGenericMethod( entityType ).Invoke( this, new object[] { context, methodName, parameters, shapers ?? Enumerable.Empty<IQueryShaper>() } ).To<EntityQuery>();
			return result;
		}
	}
}
