using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DragonSpark.Extensions
{
	public static class EnumerableExtensions
	{
		public static void CheckWith<TItem>( this IList<WeakReference> target, TItem item, Action<TItem> action )
		{
			if ( !target.AliveOnly().Exists( item ) )
			{
				target.Add( new WeakReference( item ) );
				action( item );
				// target.Loaded += new Loader<TFrameworkElement>( callback ).OnLoad;
			}
		}

		public static IEnumerable<TItem> Targets<TItem>( this IList<WeakReference> target )
		{
			var result = target.AliveOnly().Select( x => x.Target ).OfType<TItem>().ToArray();
			return result;
		}

		public static IList<WeakReference> AliveOnly( this IList<WeakReference> target )
		{
			var items = target.Where( x => !x.IsAlive ).ToArray();
			items.Apply( x => target.Remove( x ) );
			return target;
		}

		public static bool Exists( this IEnumerable<WeakReference> target, object item )
		{
			var result = target.Any( x => Equals( x.Target, item ) && x.IsAlive );
			return result;
		}
			
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Tuple is what Tuple does." )]
		public static IEnumerable<Tuple<TItem, TItem>> TupleWith<TItem>( this IEnumerable<TItem> target, IEnumerable<TItem> other )
		{
			var first = target.ToList();
			var result = target.Select( x => new Tuple<TItem, TItem>( x, other.ElementAtOrDefault( first.IndexOf( x ) ) ) ).ToArray();
			return result;
		}

		public static IEnumerable<TItem> NotNull<TItem>( this IEnumerable<TItem> target )
		{
			Contract.Requires( target != null );
			var result = target.Where( x => !Equals( x, default( TItem ) ) );
			return result;
		}
	}
}