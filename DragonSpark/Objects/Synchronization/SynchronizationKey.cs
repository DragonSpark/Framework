using System;

namespace DragonSpark.Objects.Synchronization
{
	public struct SynchronizationKey
	{
		public SynchronizationKey( Type first, Type second ) : this( first, second, null )
		{}

		public SynchronizationKey( Type first, Type second, string name ) : this()
		{
			First = first;
			Second = second;
			Name = name;
		}

		public bool IsAssignable( SynchronizationKey key )
		{
			var result = First.IsAssignableFrom( key.First ) && Second.IsAssignableFrom( key.Second ) && Equals( key.Name, Name );
			return result;
		}

		public static bool operator ==( SynchronizationKey first, SynchronizationKey second )
		{
			var result = first.Equals( second );
			return result;
		}

		public static bool operator !=( SynchronizationKey first, SynchronizationKey second )
		{
			return !( first == second );
		}

		public Type First { get; set;} 
		public Type Second { get; set; }
		public string Name { get; set; }

		public bool Equals( SynchronizationKey obj )
		{
			return Equals( obj.First, First ) && Equals( obj.Second, Second ) && Equals( obj.Name, Name );
		}

		public override bool Equals( object obj )
		{
			return obj.GetType() == typeof(SynchronizationKey) && Equals( (SynchronizationKey)obj );
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var result = ( First != null ? First.GetHashCode() : 0 );
				result = ( result * 397 ) ^ ( Second != null ? Second.GetHashCode() : 0 );
				result = ( result * 397 ) ^ ( Name != null ? Name.GetHashCode() : 0 );
				return result;
			}
		}
	}
}