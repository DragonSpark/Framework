using DragonSpark.Sources.Parameterized;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DragonSpark.Composition
{
	public sealed class SingletonExports : ExportSourceBase<SingletonExport>, IEnumerable<SingletonExport>
	{
		readonly IDictionary<Type, SingletonExport> dictionary;
		public SingletonExports( IDictionary<Type, SingletonExport> dictionary ) : base( dictionary.Keys, new DictionarySource<Type, SingletonExport>( dictionary ) )
		{
			this.dictionary = dictionary;
		}

		public IEnumerator<SingletonExport> GetEnumerator() => dictionary.Values.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}