using System;

namespace DragonSpark.Runtime
{
    public interface IDelegateWorker
    {
        IDelegateContext Execute( Action target );

        IDelegateContext Start( Action target );

        IDelegateContext Delay( Action target, TimeSpan time );
    }
}