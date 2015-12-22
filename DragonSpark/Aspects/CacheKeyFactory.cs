using System.Collections.Generic;
using System.Linq;
using DragonSpark.Activation.FactoryModel;

namespace DragonSpark.Aspects
{
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