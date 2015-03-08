using System.Collections.Generic;
using System.Linq;
using DragonSpark.Objects;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Presentation.Configuration
{
    public class ItemsFactory<T> : Factory<IEnumerable<T>> where T : class
    {
        readonly System.Type targetType;

        public ItemsFactory() : this( System.Windows.Application.Current.GetType() )
        {}

        public ItemsFactory( System.Type targetType )
        {
            this.targetType = targetType;
        }

        protected override IEnumerable<T> CreateItem( object source )
        {
            var result = targetType.Assembly.GetExportedTypes().OrderBy( x => x.Namespace.Length ).Where( typeof(T).IsAssignableFrom ).Select( Activator.CreateInstance<T> ).ToArray();
            return result;
        }
    }
}