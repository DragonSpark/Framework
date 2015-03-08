using System;
using DragonSpark.Extensions;

namespace DragonSpark.Application
{
    public static class DelegateWorkerExtensions
    {
        public static TContext ExecuteAs<TContext>( this IDelegateWorker target, Action action ) where TContext : IDelegateContext
        {
            var result = target.Execute( action ).To<TContext>();
            return result;
        }
    }
}