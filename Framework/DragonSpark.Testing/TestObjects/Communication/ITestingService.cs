using System.ServiceModel;

namespace DragonSpark.Testing.TestObjects.Communication
{
	[ServiceContract]
	interface ITestingService
	{
		[OperationContract]
		string HelloWorld( string message );
	}
}
