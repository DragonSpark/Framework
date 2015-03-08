using System.ServiceModel;

namespace DragonSpark.Application
{
	[ServiceContract( Namespace = "http://services.DragonSpark-framework.com/Application" )]
	public interface IApplicationService
	{
		[OperationContract]
		ApplicationDetails RetrieveApplicationDetails();

		[OperationContract]
		void ReportException( ClientExceptionDetail clientExceptionToReport );
	}
}