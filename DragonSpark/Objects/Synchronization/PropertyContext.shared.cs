using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Objects.Synchronization
{
	public sealed class PropertyContext
	{
		readonly object container;
		readonly PropertyInfo property;
		readonly object value;
		readonly ReadOnlyCollection<object> index;

		public PropertyContext( object container, PropertyInfo property, object value, IList<object> index )
		{
			this.container = container;
			this.property = property;
			this.value = value;
			this.index = new ReadOnlyCollection<object>( index ?? Enumerable.Empty<object>().ToList() );
		}

		public object Container
		{
			get { return container; }
		}

		public PropertyInfo Property
		{
			get { return property; }
		}

		public object Value
		{
			get { return value; }
		}

		public ReadOnlyCollection<object> Index
		{
			get { return index; }
		}

		public override bool Equals(object obj)
		{
			if ( ReferenceEquals( null, obj ) )
			{
				return false;
			}
			if ( ReferenceEquals( this, obj ) )
			{
				return true;
			}
			if ( obj.GetType() != typeof(PropertyContext) )
			{
				return false;
			}
			return Equals( (PropertyContext)obj );
		}

		public bool Equals( PropertyContext other )
		{
			if ( ReferenceEquals( null, other ) )
			{
				return false;
			}
			if ( ReferenceEquals( this, other ) )
			{
				return true;
			}
			return Equals( other.container, container ) && Equals( other.property, property ) && Equals( other.value, value ) && Equals( other.index, index );
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = ( container != null ? container.GetHashCode() : 0 );
				result = ( result * 397 ) ^ ( property != null ? property.GetHashCode() : 0 );
				result = ( result * 397 ) ^ ( value != null ? value.GetHashCode() : 0 );
				result = ( result * 397 ) ^ ( index != null ? index.GetHashCode() : 0 );
				return result;
			}
		}
	}
}