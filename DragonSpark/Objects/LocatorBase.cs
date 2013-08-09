using System;
using System.Collections.Generic;

namespace DragonSpark.Objects
{
	public abstract class LocatorBase<TSource,TObject> : ILocator
	{
		public event EventHandler<ObjectFindingEventArgs<TSource,TObject>> Finding = delegate { };

		protected virtual IEnumerable<TObject> List
		{
			get { return null; }
		}

		protected virtual TObject FindItem( TSource source )
		{
			return default(TObject);
		}

		public TObject Find( TSource source )
		{
			var args = new ObjectFindingEventArgs<TSource, TObject>( List, source ) { Result = FindItem( source ) };
			Finding( this, args );
			return args.Result;
		}

		protected virtual TSource ResolveKey( object source )
		{
			var result = (TSource)source;
			return result;
		}

		protected virtual TObject DefaultValue
		{
			get{ return default( TObject ); }
		}

		object ILocator.Find( object key )
		{
			var resolveKey = ResolveKey( key );
			var result = !Equals( resolveKey, default( TSource ) ) ? Find( resolveKey ) : DefaultValue;
			return result;
		}
	}
}