using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Runtime
{
    public class TaskContext : IDelegateContext
    {
        readonly Task task;
        readonly CancellationTokenSource token;

        internal TaskContext( Task task, CancellationTokenSource token )
        {
            this.task = task;
            this.token = token;
        }

        public Task Task
        {
            get { return task; }
        }

        public object State
        {
            get { return task; }
        }

        public bool Cancel()
        {
            token.Cancel();
            return true;
        }
    }
}