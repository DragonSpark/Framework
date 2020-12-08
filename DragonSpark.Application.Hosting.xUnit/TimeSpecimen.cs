using DragonSpark.Compose;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Hosting.xUnit
{
	sealed class TimeSpecimen : Specimen<ITime>
	{
		public static TimeSpecimen Default { get; } = new TimeSpecimen();

		TimeSpecimen() : base(Epoch.Default.Self) {}
	}
}