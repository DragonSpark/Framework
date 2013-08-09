using DragonSpark.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace DragonSpark
{
	public static class ListSupport
	{
		static IList ResolveFromListSource( object target )
		{
			var result = target.As<IListSource>().Transform( x => x.GetList() );
			return result;
		}

		static readonly MethodInfo SynchronizeMethod = typeof(ListSupport).GetMethod( "SynchronizeList", DragonSparkBindingOptions.AllProperties );

		public static void Synchronize( IList source, IList target )
		{
			var sourceType = ResolveElementType( source.GetType() );
			var targetType = ResolveElementType( target.GetType() );
			if ( sourceType.IsAssignableFrom( targetType ) )
			{
				var method = SynchronizeMethod.MakeGenericMethod( targetType );
				method.Invoke( null, new[] { source, target } );
			}
		}

		static void SynchronizeList<ItemType>( IList source, IList target )
		{
			var left = source.Cast<ItemType>();
			var right = target.Cast<ItemType>();
			var add = left.Except( right ).ToList();
			var remove = right.Except( left ).ToList();

			// Remove old items:
			remove.Apply( item => target.Remove( item ) );

			// Add new items:
			add.Apply( item => target.Add( item ) );

			// Synchronize indexes:
			foreach ( var item in source )
			{
				var sourceIndex = source.IndexOf( item );
				var targetIndex = target.IndexOf( item );
				if ( sourceIndex != targetIndex )
				{
					var targetItem = target[ targetIndex ];
					target.Remove( targetItem );
					target.Insert( sourceIndex, targetItem );
				}
			}
		}

		public static Type ResolveElementType( Type type )
		{
			if ( !type.HasElementType )
			{
				var info = type.GetProperty( "Item", DragonSparkBindingOptions.AllProperties );
				var result = info != null ? info.PropertyType : ResolveListItemType( type );
				return result;
			}
			return type.GetElementType();
		}

		public static IList ResolveList( object target )
		{
			var result = ResolveFromListSource( target ) ?? ResolveListInternal( target );
			return result;
		}

		static IList ResolveListInternal( object target )
		{
			var list = target as IList;
			if ( list != null )
			{
				return list;
			}
			var collection = target as ICollection;
			var result = collection != null ? collection.Cast<object>().ToArray() : null;
			return result;
		}

		static Type ResolveListItemType( Type type )
		{
			if ( type.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom( type.GetGenericTypeDefinition() ) )
			{
				var arguments = type.GetGenericArguments();
				return arguments.Length == 1 ? arguments[ 0 ] : null;
			}
			return null;
		}
	}
}
