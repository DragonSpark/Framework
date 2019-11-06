using System;

namespace DragonSpark.Model.Sequences.Query.Construction
{
	sealed class ReturnedContent<TIn, TOut> : IContent<TIn, TOut>
	{
		readonly IContent<TIn, TOut> _content;
		readonly Action<TIn[]>       _return;

		public ReturnedContent(IContent<TIn, TOut> content) : this(content, Return<TIn>.Default.Execute) {}

		public ReturnedContent(IContent<TIn, TOut> content, Action<TIn[]> @return)
		{
			_content = content;
			_return  = @return;
		}

		public Store<TOut> Get(Store<TIn> parameter)
		{
			var result = _content.Get(parameter);
			if (parameter.Requested)
			{
				_return(parameter.Instance);
			}

			return result;
		}
	}
}