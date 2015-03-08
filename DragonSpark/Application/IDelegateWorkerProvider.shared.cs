namespace DragonSpark.Application
{
    public interface IDelegateWorkerProvider
    {
        IDelegateWorker Primary { get; }
        IDelegateWorker Secondary { get; }
    }
}