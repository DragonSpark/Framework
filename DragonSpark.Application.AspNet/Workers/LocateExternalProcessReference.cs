using DragonSpark.Application.AspNet.Entities;

namespace DragonSpark.Application.AspNet.Workers;

public sealed class LocateExternalProcessReference : Locate<Guid, ExternalProcess>
{
	public LocateExternalProcessReference(IEnlistedScopes scopes)
		: base(scopes, x => x.Id, SelectExternalProcess.Default) {}
}