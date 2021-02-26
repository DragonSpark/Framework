using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.ElasticEmail
{
	sealed class Initializer : FixedParameterCommand<string>
	{
		public Initializer(ElasticEmailSettings configuration)
			: this(configuration.ApiKey, RegisterLicense.Default.Execute) {}

		public Initializer(string configuration, Action<string> command) : base(command, configuration) {}
	}
}