using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;

namespace DragonSpark.Extensions
{
	public static class DynamicExtensions
	{
		readonly static IEnumerable<CSharpArgumentInfo> CSharpArgumentInfos = CSharpArgumentInfo.Create( CSharpArgumentInfoFlags.None, null ).ToEnumerable();
		readonly static IDictionary<Tuple<Type,string>,CallSite<Func<CallSite, object, object>>> Cache = new Dictionary<Tuple<Type, string>, CallSite<Func<CallSite, object, object>>>();

		public static object GetValue( this IDynamicMetaObjectProvider target, string name)
		{
			var key = new Tuple<Type, string>( target.GetType(), name );
			var site = Cache.Ensure( key, x => CallSite<Func<CallSite, object, object>>.Create( Binder.GetMember( CSharpBinderFlags.None, x.Item2, x.Item1, CSharpArgumentInfos ) ) );
			var result = site.Target( site, target );
			return result; 
		} 
	}
}