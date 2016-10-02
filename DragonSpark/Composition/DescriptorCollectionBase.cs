using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.TypeSystem;

namespace DragonSpark.Composition
{
	public abstract class DescriptorCollectionBase<T> : KeyedCollection<Type, T>
	{
		readonly Func<T, Type> valueSource;

		protected DescriptorCollectionBase( IEnumerable<T> items, Func<T, Type> valueSource )
		{
			this.valueSource = valueSource;
			this.AddRange( items );
		}

		public IEnumerable<Type> Keys => Dictionary?.Keys ?? Items<Type>.Default;

		public IEnumerable<Type> Values => Dictionary?.Values.Select( valueSource ) ?? Items<Type>.Default;

		public IEnumerable<Type> All() => Keys.Concat( Values ).Distinct();
	}
}