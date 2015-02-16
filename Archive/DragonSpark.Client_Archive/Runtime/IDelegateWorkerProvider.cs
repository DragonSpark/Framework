namespace DragonSpark.Runtime
{
    public interface IDelegateWorkerProvider
    {
        IDelegateWorker Primary { get; }
        IDelegateWorker Secondary { get; }
    }
}