namespace DragonSpark.Application
{
    public interface IDelegateContext
    {
        object State { get; }

        bool Cancel();
    }
}