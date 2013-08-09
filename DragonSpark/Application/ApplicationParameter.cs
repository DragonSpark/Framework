using System;

namespace DragonSpark.Application
{
	public struct ApplicationParameter
	{
		public Type ParameterType { get; set; }
		public string ParameterName { get; set; }

		public ApplicationParameter( object owner, string name ) : this( owner.GetType(), name )
		{}

		public ApplicationParameter( Type type, string name ) : this()
		{
			ParameterType = type;
			ParameterName = name;
		}

		public bool Equals( ApplicationParameter other )
		{
			return other.ParameterType == ParameterType && Equals( other.ParameterName, ParameterName );
		}

		public override bool Equals( object obj )
		{
			var result = !ReferenceEquals( null, obj ) &&
			              ( obj.GetType() == typeof(ApplicationParameter) && Equals( (ApplicationParameter)obj ) );
			return result;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ( ( ParameterType != null ? ParameterType.GetHashCode() : 0 ) * 397 ) ^ ( ParameterName != null ? ParameterName.GetHashCode() : 0 );
			}
		}

		public static bool operator ==( ApplicationParameter left, ApplicationParameter right )
		{
			return left.Equals( right );
		}

		public static bool operator !=( ApplicationParameter left, ApplicationParameter right )
		{
			return !left.Equals( right );
		}

		public override string ToString()
		{
			return string.Format( "{0}.{1}", ParameterType.FullName, ParameterName );
		}
	}
}