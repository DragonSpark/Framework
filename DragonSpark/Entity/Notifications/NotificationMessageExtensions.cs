using System.Data.Entity;
using System.Linq;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Communication.Entity.Notifications
{
    public static class NotificationMessageExtensions
    {
        public static NotificationMessage Applied( this NotificationMessage target, DbContext context )
        {
            target.To.ToArray().Union( new [] { target.From, target.ReplacementTarget } ).NotNull().Apply( x => context.ApplyChanges( x ) );
            return target;
        }
    }
}