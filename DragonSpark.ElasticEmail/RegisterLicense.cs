using DragonSpark.Model.Commands;
using ElasticEmailClient;

namespace DragonSpark.ElasticEmail
{
	sealed class RegisterLicense : ICommand<string>
	{
		public static RegisterLicense Default { get; } = new RegisterLicense();

		RegisterLicense() {}

		public void Execute(string parameter)
		{
			Api.ApiKey = parameter;
		}
	}
}