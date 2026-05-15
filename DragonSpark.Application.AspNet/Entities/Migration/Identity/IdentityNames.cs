using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class IdentityNames : Instances<string>
{
	public static IdentityNames Default { get; } = new();

	IdentityNames() : base("Id", "Identity") {}
}