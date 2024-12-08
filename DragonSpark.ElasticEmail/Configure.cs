using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.ElasticEmail;

sealed class Configure : IAlteration<ApplicationProfileContext>
{
	public static Configure Default { get; } = new();

	Configure() {}

	public ApplicationProfileContext Get(ApplicationProfileContext parameter)
		=> parameter.Append(Registrations.Default)
		            .Append(ApplicationConfiguration.Default);
}