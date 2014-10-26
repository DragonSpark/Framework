using System.Collections.Generic;
using System.Linq;
using DragonSpark.IoC;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Runtime
{
    [Singleton( typeof(IDelegateWorkerProvider), Priority = Priority.Lowest )]
    public class ServiceLocationDelegateWorkerProvider : IDelegateWorkerProvider
    {
        IEnumerable<IDelegateWorker> Workers
        {
            get { return workers ?? ( workers = ServiceLocator.Current.GetAllInstances<IDelegateWorker>().ToArray() ); }
        }	IEnumerable<IDelegateWorker> workers;
	
        public IDelegateWorker Primary
        {
            get { return primary ?? ( primary = Workers.FirstOrDefault() ); }
        }	IDelegateWorker primary;

        public IDelegateWorker Secondary
        {
            get { return secondary ?? ( secondary = Workers.LastOrDefault() ); }
        }	IDelegateWorker secondary;
    }
}