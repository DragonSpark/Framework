using DragonSpark.Model.Commands;
using Syncfusion.Licensing;

namespace DragonSpark.SyncfusionRendering;

sealed class RegisterLicense : Command<string>
{
	public static RegisterLicense Default { get; } = new RegisterLicense();

	RegisterLicense() : base(SyncfusionLicenseProvider.RegisterLicense) {}
}