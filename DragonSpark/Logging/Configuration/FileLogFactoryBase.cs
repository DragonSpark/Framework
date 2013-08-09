namespace DragonSpark.Logging.Configuration
{
	public abstract class FileLogFactoryBase : EventListenerFactory
	{
		public string LogFilePath { get; set; }

		public bool IsAsynchronus { get; set; }
	}
}