using System;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Query.Construction
{
	sealed class Exit<_, T> : ISelect<_, T[]>
	{
		readonly static Action<T[]>          Return = Return<T>.Default.Execute;
		readonly        IBody<T>             _body;
		readonly        ISelect<_, Store<T>> _origin;
		readonly        Action<T[]>          _return;

		public Exit(ISelect<_, T[]> origin, IBody<T> body) : this(origin, body, Return) {}

		public Exit(ISelect<_, T[]> origin, IBody<T> body, Action<T[]> @return)
			: this(new Origin<_, T>(origin), body, @return) {}

		public Exit(ISelect<_, Store<T>> origin, IBody<T> body) : this(origin, body, Return) {}

		public Exit(ISelect<_, Store<T>> origin, IBody<T> body, Action<T[]> @return)
		{
			_origin = origin;
			_body   = body;
			_return = @return;
		}

		public T[] Get(_ parameter)
		{
			var storage = _origin.Get(parameter);
			var result = _body.Get(new ArrayView<T>(storage.Instance, 0, storage.Length))
			                  .ToArray();

			if (storage.Requested)
			{
				_return(storage.Instance);
			}

			return result;
		}
	}

	sealed class Exit<_, TIn, TOut> : ISelect<_, TOut[]>
	{
		readonly ISelect<Store<TOut>, TOut[]> _complete;
		readonly IContent<TIn, TOut>          _content;
		readonly ISelect<_, Store<TIn>>       _origin;

		public Exit(ISelect<_, Store<TIn>> origin, IContent<TIn, TOut> content)
			: this(origin, content, Complete<TOut>.Default) {}

		public Exit(ISelect<_, Store<TIn>> origin, IContent<TIn, TOut> content,
		            ISelect<Store<TOut>, TOut[]> complete)
		{
			_origin   = origin;
			_content  = content;
			_complete = complete;
		}

		public TOut[] Get(_ parameter) => _complete.Get(_content.Get(_origin.Get(parameter)));
	}

	sealed class Exit<_, TIn, TOut, TTo> : ISelect<_, TTo>
	{
		readonly IContent<TIn, TOut>    _content;
		readonly ISelect<_, Store<TIn>> _origin;
		readonly IReduce<TOut, TTo>     _reduce;

		public Exit(ISelect<_, Store<TIn>> origin, IContent<TIn, TOut> content, IReduce<TOut, TTo> reduce)
		{
			_origin  = origin;
			_content = content;
			_reduce  = reduce;
		}

		public TTo Get(_ parameter) => _reduce.Get(_content.Get(_origin.Get(parameter)));
	}
}