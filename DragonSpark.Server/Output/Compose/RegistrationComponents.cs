using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.OutputCaching;

namespace DragonSpark.Server.Output.Compose;

public readonly record struct RegistrationComponents(
	IOutputCacheStore Store,
	Array<IOutputKey> Keys,
	List<IRegistration> Registrations)
{
	public RegistrationComponents(IOutputCacheStore Store, Array<IOutputKey> Keys) : this(Store, Keys, []) {}
}