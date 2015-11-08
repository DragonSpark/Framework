namespace DragonSpark.Testing.TestObjects.Communication
{
	class TestingService : ITestingService
	{
		readonly string baselineMessage;

		public TestingService( string baselineMessage )
		{
			this.baselineMessage = baselineMessage;
		}

		public string HelloWorld( string message )
		{
			var result = string.Format( "{0}: {1}", baselineMessage,  message );
			return result;
		}
	}
}