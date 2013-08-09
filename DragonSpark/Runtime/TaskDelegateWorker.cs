using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Extensions;
using DragonSpark.IoC;

namespace DragonSpark.Runtime
{
    [Singleton( typeof(IDelegateWorker), Name = "TaskDelegateWorker", Priority = Priority.Lowest )]
    public class TaskDelegateWorker : IDelegateWorker
    {
        public IDelegateContext Execute( Action target )
        {
            var result = Start( target );
            return result;
        }

        public IDelegateContext Start( Action target )
        {
            var result = ResolveContext( target );
            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Token source gets disposed when the Task does." )]
        static TaskContext ResolveContext( Delegate target )
        {
            var source = new CancellationTokenSource();
            var task = Task.Factory.StartNew( () => target.DynamicInvoke(), source.Token ).ContinueWith( x => x.Exception.NotNull( y => { throw y.InnerException; } ) );
            var result = new TaskContext( task, source );
            return result;
        }

        public IDelegateContext Delay( Action target, TimeSpan time )
        {
            var result = ResolveContext( target );
            result.Task.ContinueWith( x => Thread.Sleep( time ) );
            return result;
        }
    }
}