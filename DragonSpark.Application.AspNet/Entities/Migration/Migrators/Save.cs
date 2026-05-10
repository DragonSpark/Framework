using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class Save<T> : ISave<T> where T : class
{
	public static Save<T> Default { get; } = new();

	Save() {}

	public async ValueTask<uint> Get(Stop<SaveInput<T>> parameter)
	{
		var ((_, _, destination, _, _), stop) = parameter;
		return (uint)await destination.SaveChangesAsync(stop).Off();
	}
}