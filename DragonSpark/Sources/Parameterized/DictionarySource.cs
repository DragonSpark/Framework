using System.Collections.Generic;

namespace DragonSpark.Sources.Parameterized
{
	public class DictionarySource<TParameter, TResult> : ParameterizedSourceBase<TParameter, TResult>
	{
		readonly IDictionary<TParameter, TResult> dictionary;
		public DictionarySource( IDictionary<TParameter, TResult> dictionary )
		{
			this.dictionary = dictionary;
		}

		public override TResult Get( TParameter parameter )
		{
			TResult result;
			return dictionary.TryGetValue( parameter, out result ) ? result : default(TResult);
		}
	}
}