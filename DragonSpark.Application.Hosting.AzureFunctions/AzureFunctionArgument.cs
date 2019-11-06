using DragonSpark.Runtime.Activation;

namespace DragonSpark.Application.Hosting.AzureFunctions
{
	public sealed class AzureFunctionArgument : ApplicationArgument<AzureFunctionParameter>,
	                                            IActivateUsing<AzureFunctionParameter>
	{
		public AzureFunctionArgument(AzureFunctionParameter instance) : base(instance) {}
	}
}