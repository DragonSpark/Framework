using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public interface IBuildPropertyRepository
	{
		IEnumerable<DefaultValueParameter> GetProperties( object instance );

		void MarkBuilt( DefaultValueParameter property );
	}

	// [Synchronized]
	public class BuildPropertyRepository : IBuildPropertyRepository
	{
		public static BuildPropertyRepository Instance { get; } = new BuildPropertyRepository();

		class IsBuilt : ConnectedValue<bool>
		{
			public IsBuilt( object instance ) : base( instance, typeof(IsBuilt) )
			{}
		}

		class Properties : ConnectedValue<ICollection<PropertyInfo>>
		{
			public Properties( object instance ) : base( instance, typeof(Properties), () => BuildablePropertyCollectionFactory.Instance.Create( instance ) )
			{}
		}

		/*public static void Reset( object instance )
		{
			new IsBuilt( instance ).Property.TryDisconnect();
			new Properties( instance ).Property.TryDisconnect();
		}*/

		public IEnumerable<DefaultValueParameter> GetProperties( object instance )
		{
			var result = !new IsBuilt( instance ).Item ? new Properties( instance ).Item.Select( info => new DefaultValueParameter( instance, info ) ).ToArray() : Enumerable.Empty<DefaultValueParameter>();
			return result;
		}

		public void MarkBuilt( DefaultValueParameter property )
		{
			var collection = new Properties( property.Instance ).Item;
			collection.Remove( property.Metadata );
			collection.Any().IsFalse( () => new IsBuilt( property.Instance ).Assign( true ) );
		}
	}
}