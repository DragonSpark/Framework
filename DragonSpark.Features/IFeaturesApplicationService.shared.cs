using System.ServiceModel;

namespace DragonSpark.Features
{
	[ServiceContract( Namespace = "http://services.common-framework.com/Features" )]
	public interface IFeaturesApplicationService
	{
		[OperationContract]
		void ThrowException();
	}
}