using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Save;

sealed class EmptySave : ISave
{
	public static EmptySave Default { get; } = new();

	EmptySave() {}

	public ValueTask<uint> Get(Stop<SaveInput> parameter) => new(0);

	public uint? Get() => null;
}