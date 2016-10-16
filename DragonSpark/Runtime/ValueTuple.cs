using System;
using System.Collections.Generic;

namespace DragonSpark.Runtime
{
	// ATTRIBUTION: https://github.com/dotnet/roslyn/blob/master/src/Compilers/Core/Portable/InternalUtilities/ValueTuple.cs
	public static class ValueTuple
	{
		public static ValueTuple<T1, T2> Create<T1, T2>( T1 item1, T2 item2 ) => new ValueTuple<T1, T2>( item1, item2 );

		public static ValueTuple<T1, T2, T3> Create<T1, T2, T3>( T1 item1, T2 item2, T3 item3 ) => new ValueTuple<T1, T2, T3>( item1, item2, item3 );
	}

	// ATTRIBUTION: https://github.com/dotnet/roslyn/blob/master/src/Compilers/Core/Portable/InternalUtilities/ValueTuple%603.cs
	public struct ValueTuple<T1, T2, T3> : IEquatable<ValueTuple<T1, T2, T3>>
	{
		readonly static EqualityComparer<T1> Comparer1 = EqualityComparer<T1>.Default;
		readonly static EqualityComparer<T2> Comparer2 = EqualityComparer<T2>.Default;
		readonly static EqualityComparer<T3> Comparer3 = EqualityComparer<T3>.Default;

		public ValueTuple( T1 item1, T2 item2, T3 item3 )
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
		}

		public T1 Item1 { get; }
		public T2 Item2 { get; }
		public T3 Item3 { get; }

		public bool Equals( ValueTuple<T1, T2, T3> other ) => Comparer1.Equals( Item1, other.Item1 )
															  && Comparer2.Equals( Item2, other.Item2 )
															  && Comparer3.Equals( Item3, other.Item3 );

		public override bool Equals( object obj )
		{
			if ( obj is ValueTuple<T1, T2, T3> )
			{
				var other = (ValueTuple<T1, T2, T3>)obj;
				return Equals( other );
			}

			return false;
		}

		public override int GetHashCode() => Hash.Combine(
			Hash.Combine(
				Comparer1.GetHashCode( Item1 ),
				Comparer2.GetHashCode( Item2 ) ),
			Comparer3.GetHashCode( Item3 ) );

		public static bool operator ==( ValueTuple<T1, T2, T3> left, ValueTuple<T1, T2, T3> right ) => left.Equals( right );

		public static bool operator !=( ValueTuple<T1, T2, T3> left, ValueTuple<T1, T2, T3> right ) => !left.Equals( right );
	}

	// ATTRIBUTION: https://github.com/dotnet/roslyn/blob/master/src/Compilers/Core/Portable/InternalUtilities/ValueTuple%602.cs
	public struct ValueTuple<T1, T2> : IEquatable<ValueTuple<T1, T2>>
	{
		readonly static EqualityComparer<T1> Comparer1 = EqualityComparer<T1>.Default;
		readonly static EqualityComparer<T2> Comparer2 = EqualityComparer<T2>.Default;

		public ValueTuple( T1 item1, T2 item2 )
		{
			Item1 = item1;
			Item2 = item2;
		}

		public T1 Item1 { get; }
		public T2 Item2 { get; }

		public bool Equals( ValueTuple<T1, T2> other ) => Comparer1.Equals( Item1, other.Item1 ) && Comparer2.Equals( Item2, other.Item2 );

		public override bool Equals( object obj )
		{
			if ( obj is ValueTuple<T1, T2> )
			{
				var other = (ValueTuple<T1, T2>)obj;
				return Equals( other );
			}

			return false;
		}

		public override int GetHashCode() => Hash.Combine( Comparer1.GetHashCode( Item1 ), Comparer2.GetHashCode( Item2 ) );

		public static bool operator ==( ValueTuple<T1, T2> left, ValueTuple<T1, T2> right ) => left.Equals( right );

		public static bool operator !=( ValueTuple<T1, T2> left, ValueTuple<T1, T2> right ) => !left.Equals( right );
	}
}