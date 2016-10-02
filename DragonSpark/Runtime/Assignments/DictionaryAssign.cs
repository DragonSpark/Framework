using System.Collections.Generic;

namespace DragonSpark.Runtime.Assignments
{
	public class DictionaryAssign<T1, T2> : IAssign<T1, T2>
	{
		readonly IDictionary<T1, T2> dictionary;

		public DictionaryAssign( IDictionary<T1, T2> dictionary )
		{
			this.dictionary = dictionary;
		}

		public void Assign( T1 first, T2 second ) => dictionary[first] = second;
	}
}