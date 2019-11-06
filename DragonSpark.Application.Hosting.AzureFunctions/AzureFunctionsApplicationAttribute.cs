using DragonSpark.Runtime.Environment;

namespace DragonSpark.Application.Hosting.AzureFunctions
{
	public sealed class AzureFunctionsApplicationAttribute : HostingAttribute
	{
		public AzureFunctionsApplicationAttribute() : base(typeof(AzureFunctionsApplicationAttribute).Assembly) {}
	}
}