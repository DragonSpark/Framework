using System.Linq;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Regions;

namespace DragonSpark.Application.Presentation.Extensions
{
	public static class RegionExtensions
	{
		public static TBehavior GetBehavior<TBehavior>( this IRegion target )
		{
			var result = target.Behaviors.Select( x => x.Value ).FirstOrDefaultOfType<TBehavior>();
			return result;
		}
	}
}