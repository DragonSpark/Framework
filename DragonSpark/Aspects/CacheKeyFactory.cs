using System.Collections.Generic;
using System.Linq;
using DragonSpark.Activation.FactoryModel;
using PostSharp.Aspects;

namespace DragonSpark.Aspects
{
	/*public class MethodCacheKeyFactory : FactoryBase<MethodInterceptionArgs, string>
	{
		protected override string CreateItem( MethodInterceptionArgs parameter )
		{
			CacheKeyFactory.Instance.Create( DetermineHost( args ).Append( args.Method ).Concat( args.Arguments ) )
		}
	}*/

	public class CacheKeyFactory : FactoryBase<IEnumerable<object>, string>
	{
		public static CacheKeyFactory Instance { get; } = new CacheKeyFactory();

		protected override string CreateItem( IEnumerable<object> parameter )
		{
			var result = string.Join( "_", parameter.Select( o => o?.GetHashCode() ?? -1 ) );
			return result;
		}
	}
}