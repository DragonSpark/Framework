using DragonSpark.Compose;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Azure.Storage.Uploads;

public abstract class ClearTemporaryFile : StopAware<string>
{
	protected ClearTemporaryFile(IContainer container) : this(container.Delete()) {}

	protected ClearTemporaryFile(IDelete delete) : base(delete.Then().Terminate()) {}
}