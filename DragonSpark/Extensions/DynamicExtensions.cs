using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Binder = Microsoft.CSharp.RuntimeBinder.Binder;

namespace DragonSpark.Extensions
{
	public static class DynamicExtensions
	{
		static readonly IEnumerable<CSharpArgumentInfo> CSharpArgumentInfos = CSharpArgumentInfo.Create( CSharpArgumentInfoFlags.None, null ).ToEnumerable();
		static readonly IDictionary<Tuple<Type, string>, CallSite<Func<CallSite, object, object>>> Cache = new Dictionary<Tuple<Type, string>, CallSite<Func<CallSite, object, object>>>();

		public static object GetValue( this IDynamicMetaObjectProvider target, string name )
		{
			var key = new Tuple<Type, string>( target.GetType(), name );
			var site = Cache.Ensure( key, x => CallSite<Func<CallSite, object, object>>.Create( Binder.GetMember( CSharpBinderFlags.None, x.Item2, x.Item1, CSharpArgumentInfos ) ) );
			var result = site.Target( site, target );
			return result;
		}

		public static MemberInfo GetMemberInfo( this Expression expression )
		{
			var lambda = (LambdaExpression)expression;

			var result = lambda.Body.AsTo<UnaryExpression, Expression>( x => x.Operand, () => lambda.Body ).To<MemberExpression>().Transform( x => x.Member );
			return result;
		}
	}
}