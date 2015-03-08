using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.Objects;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Presentation.Configuration
{
	public class InstanceFactory<T> : Factory<T> where T : class
	{
		readonly System.Type targetType;

		public InstanceFactory() : this( System.Windows.Application.Current.GetType() )
		{}

		public InstanceFactory( System.Type targetType )
		{
			this.targetType = targetType;
		}

		protected override T CreateItem( object source )
		{
			var firstOrDefault = targetType.Assembly.GetExportedTypes().OrderBy( x => x.Namespace.Length ).FirstOrDefault( typeof(T).IsAssignableFrom );
			var result = firstOrDefault.Transform( Activator.CreateInstance<T> );
			return result;
		}
	}
}