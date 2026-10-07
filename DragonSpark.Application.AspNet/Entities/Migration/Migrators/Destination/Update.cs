using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Instances;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

sealed class Update<TFrom, TTo> : PageBase<TFrom, TTo> where TFrom : class where TTo : class
{
	public Update(IMap map) : base(LocateAwareEntry<TFrom, TTo>.Default, map) {}
}