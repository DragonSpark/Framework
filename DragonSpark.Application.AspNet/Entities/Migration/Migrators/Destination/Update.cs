using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Instances;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

sealed class Update<TFrom, TTo> : DestinationBase<TFrom, TTo> where TFrom : class where TTo : class
{
	public Update(IMap map) : base(LocateAwareInstance<TFrom, TTo>.Default, map) {}
}