using System;
using System.Collections.Immutable;
using DragonSpark.Runtime;
using PostSharp.Aspects;

namespace DragonSpark.Aspects
{
	public struct CacheEntry : IEquatable<CacheEntry>
	{
		readonly int code;
		readonly MethodInterceptionArgs factory;

		public CacheEntry( MethodInterceptionArgs args ) : this( KeyFactory.Create( args.Arguments.ToImmutableArray() ), args ) {}

		public CacheEntry( int code, MethodInterceptionArgs factory )
		{
			this.code = code;
			this.factory = factory;
		}

		public object Get() => factory.GetReturnValue();

		public bool Equals( CacheEntry other ) => code == other.code;

		public override bool Equals( object obj ) => !ReferenceEquals( null, obj ) && ( obj is CacheEntry && Equals( (CacheEntry)obj ) );

		public override int GetHashCode() => code;

		public static bool operator ==( CacheEntry left, CacheEntry right ) => left.Equals( right );

		public static bool operator !=( CacheEntry left, CacheEntry right ) => !left.Equals( right );
	}
}