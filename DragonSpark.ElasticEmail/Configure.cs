using DragonSpark.Application.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.ElasticEmail
{
	sealed class Configure : IAlteration<ApplicationProfileContext>
	{
		public static Configure Default { get; } = new Configure();

		Configure() {}

		public ApplicationProfileContext Get(ApplicationProfileContext parameter)
			=> parameter.Then(Registrations.Default)
			            .Then(ApplicationConfiguration.Default);
	}
}