using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Save;

sealed class Save : ISave
{
	public static Save Default { get; } = new();

	Save() {}

	public async ValueTask<uint> Get(Stop<SaveInput> parameter)
	{
		var ((_, _, destination, _), stop) = parameter;
		return (uint)await destination.SaveChangesAsync(stop).Off();
	}
}