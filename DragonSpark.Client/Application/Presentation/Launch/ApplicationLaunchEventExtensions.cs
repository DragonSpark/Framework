using System;
using System.Linq;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Events;

namespace DragonSpark.Application.Presentation.Launch
{
    public static class ApplicationLaunchEventExtensions
    {
        public static void ExecuteWhenStatusIs( this IEventAggregator target, ApplicationLaunchStatus status, Action action )
        {
            var ready = target.GetEvent<ApplicationLaunchEvent>().History.Transform( y => Enumerable.Contains( y, status ) );
            if ( ready )
            {
                action();
            }
            else
            {
                target.Subscribe<ApplicationLaunchEvent,ApplicationLaunchStatus>( ( e, p ) =>
                {
                    var result = p == status;
                    result.IsTrue( action );
                    return result;
                } );
            }
        }
    }
}