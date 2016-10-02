using DragonSpark.Commands;
using System;

namespace DragonSpark.Diagnostics
{
	public class DelegatedTextCommand : DelegatedCommand<string>
	{
		public DelegatedTextCommand( Action<string> action ) : base( action ) {}
	}
}