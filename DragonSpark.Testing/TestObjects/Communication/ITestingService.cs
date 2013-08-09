using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace DragonSpark.Testing.TestObjects.Communication
{
	[ServiceContract]
	interface ITestingService
	{
		[OperationContract]
		string HelloWorld( string message );
	}
}
