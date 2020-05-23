using DragonSpark.Composition;
using DragonSpark.Model.Operations;
using System;
using System.Threading;

namespace DragonSpark.Application
{
	sealed class Token : IToken, IDisposable
	{
		readonly CancellationTokenSource _source;
		readonly CancellationToken       _token;

		public Token(CancellationTokenSource source) : this(source, source.Token) {}

		[Candidate(false)]
		public Token(CancellationTokenSource source, CancellationToken token)
		{
			_source = source;
			_token  = token;
		}

		public CancellationToken Get()
		{
			var result = _token;
			result.ThrowIfCancellationRequested();
			return result;
		}

		public void Dispose()
		{
			_source.Cancel();
		}
	}
}
