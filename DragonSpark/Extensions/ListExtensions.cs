using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using DragonSpark.ComponentModel;

namespace DragonSpark.Extensions
{
	public static class ListExtensions
	{
		static readonly MethodInfo SynchronizeMethod = typeof(ListExtensions).GetMethod( "Synchronize", DragonSparkBindingOptions.AllProperties );

		public static void SynchronizeItems( this ICollection source, ICollection target )
		{
			var sourceType = ResolveElementType( source.GetType() );
			var targetType = ResolveElementType( target.GetType() );
			if ( sourceType.IsAssignableFrom( targetType ) )
			{
				var method = SynchronizeMethod.MakeGenericMethod( targetType );
				method.Invoke( null, new object[] { source, target } );
			}
		}

		public static void Synchronize<TType>( this ICollection<TType> source, ICollection<TType> target )
		{
			var add = source.Except( target ).ToList();
			var remove = target.Except( source ).ToList();

			// Remove old items:
			remove.Apply( item => target.Remove( item ) );

			// Add new items:
			add.Apply( target.Add );

			// Synchronize indexes:
			target.As<IList<TType>>( x => source.As<IList<TType>>( y => source.Apply( item =>
			{
				var sourceIndex = y.IndexOf( item );
				var targetIndex = x.IndexOf( item );
				if ( sourceIndex != targetIndex )
				{
					var targetItem = x[ targetIndex ];
					x.Remove( targetItem );
					x.Insert( sourceIndex, targetItem );
				}
			} ) ) );
		}

		static Type ResolveElementType( Type type )
		{
			if ( !type.HasElementType )
			{
				var info = type.GetProperty( "Item", DragonSparkBindingOptions.AllProperties );
				var result = info != null ? info.PropertyType : ResolveListItemType( type );
				return result;
			}
			return type.GetElementType();
		}

		static Type ResolveListItemType( Type type )
		{
			var result = type.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom( type.GetGenericTypeDefinition() ) ? type.GetGenericArguments().FirstOrDefault() : null;
			return result;
		}

		public static void Ensure<TItem>( this IList<TItem> list, TItem item )
		{
			if ( !list.Contains( item ) )
			{
				list.Add( item );
			}
		}

		public static void RemoveAll<T>(this IList<T> list, Func<T, bool> filter)
		{
			for ( var i = 0; i < list.Count; i++ )
			{
				if ( filter( list[ i ] ) )
				{
					list.Remove( list[ i ] );
				}
			}
		}

		public static ObservableCollection<TItem> ToObservableCollection<TItem>( this IEnumerable<TItem> target )
		{
			var result = new ObservableCollection<TItem>( target );
			return result;
		}

		public static TList AddAll<TList,TItem>( this TList target, IEnumerable<TItem> items ) where TList : class, IList<TItem>
		{
			items.Apply( target.Add );

			return target;
		}
	}
}
