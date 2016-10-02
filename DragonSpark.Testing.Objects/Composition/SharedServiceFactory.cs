using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized.Caching;
using System.Composition;

namespace DragonSpark.Testing.Objects.Composition
{
	[Export, Shared]
	public class SharedServiceFactory : SourceBase<ISharedService>
	{
		public override ISharedService Get() => new SharedService().WithSelf( service => Condition.Default.Get( service ).Apply() );
	}
}