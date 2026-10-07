using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Instances;

sealed class New<TFrom, TTo> : PageBase<TFrom, TTo> where TFrom : class where TTo : class
{
	public New(IMap map) : base(Activate<TFrom, TTo>.Default, map) {}
}