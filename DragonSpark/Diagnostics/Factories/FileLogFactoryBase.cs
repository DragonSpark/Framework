namespace DragonSpark.Logging.Factories
{
	public abstract class FileLogFactoryBase : EventListenerFactory
	{
		public string LogFilePath { get; set; }

		public bool IsAsynchronous { get; set; }
	}
}