using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized.Caching;
using System.Composition;

namespace DragonSpark.Testing.Objects.Composition
{
	[Export]
	public class BasicServiceFactory : SourceBase<IBasicService>
	{
		public override IBasicService Get() => new BasicService().WithSelf( service => Condition.Default.Get( service ).Apply() );
	}
}