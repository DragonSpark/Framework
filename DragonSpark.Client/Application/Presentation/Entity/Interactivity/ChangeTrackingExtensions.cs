using System.ComponentModel;
using System.Linq;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Entity.Interactivity
{
    public static class ChangeTrackingExtensions
    {
        public static bool IsChanged( this IChangeTracking target )
        {
            var result = target.IsChanged || target.GetType().GetProperties().Where( x => typeof(IChangeTracking).IsAssignableFrom( x.PropertyType ) ).Select( x => x.GetValue( target, null ).As<IChangeTracking>() ).NotNull().Any( x => x.IsChanged() );
            return result;
        }
    }
}