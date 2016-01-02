using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Markup
{
	class Incrementer : IIncrementer
	{
		readonly IDictionary<WeakReference, int>  items = new Dictionary<WeakReference, int>();

		public int Next( object context )
		{
			var key = Items.Keys.SingleOrDefault( reference => reference.Target == context ) ?? new WeakReference( context );
			var current = items.Ensure( key, reference => 0 ) + 1;
			var result = items[key] = current;
			return result;
		}

		IDictionary<WeakReference, int> Items
		{
			get
			{
				items.Keys.Where( reference => !reference.IsAlive ).ToArray().Each( reference => items.Remove( reference ) );
				return items;
			}
		}
	}
}