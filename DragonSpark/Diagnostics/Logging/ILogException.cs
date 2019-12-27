namespace DragonSpark.Diagnostics.Logging
{
	public interface ILogException<T> : ILogMessage<ExceptionParameter<T>> {}
}