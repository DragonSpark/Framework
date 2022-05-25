using DragonSpark.Model.Commands;
using Syncfusion.Licensing;
using System;

namespace DragonSpark.SyncfusionRendering;

sealed class Initializer : FixedParameterCommand<string>
{
	public Initializer(SyncfusionConfiguration configuration)
		: this(configuration.License, SyncfusionLicenseProvider.RegisterLicense) {}

	public Initializer(string configuration, Action<string> command) : base(command, configuration) {}
}