using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Syncfusion
{
	sealed class Initializer : FixedParameterCommand<string>
	{
		public Initializer(SyncfusionConfiguration configuration)
			: this(configuration.License, RegisterLicense.Default.Execute) {}

		public Initializer(string configuration, Action<string> command) : base(command, configuration) {}
	}
}