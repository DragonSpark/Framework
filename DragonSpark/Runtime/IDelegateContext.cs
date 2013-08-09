namespace DragonSpark.Runtime
{
    public interface IDelegateContext
    {
        object State { get; }

        bool Cancel();
    }
}