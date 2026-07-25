using DragonSpark.Application.AspNet.Entities.Queries.Composition;

namespace DragonSpark.Application.AspNet.Workers;

sealed class SelectExternalProcess : StartWhere<Guid, ExternalProcess>
{
	public static SelectExternalProcess Default { get; } = new();

	SelectExternalProcess() : base((p, x) => x.Id == p) {}
}