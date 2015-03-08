using System.Collections;
using System.ComponentModel;
using DragonSpark.Extensions;

namespace DragonSpark
{
	partial class ListHelper
	{
		static IList ResolveFromListSource( object target )
		{
			var result = target.As<IListSource>().Transform( x => x.GetList() );
			return result;
		}
	}
}
