using System;
using System.Diagnostics.Contracts;

namespace DragonSpark.Runtime
{
	public sealed class DisposableActionContext : IDisposable
	{
		readonly Action action;

		public DisposableActionContext( Action action )
		{
			Contract.Requires( action != null );
			this.action = action;
		}

		/*[ContractInvariantMethod]
		void Invariant()
		{
			Contract.Invariant( action != null );
		}
		*/
		public void Dispose()
		{
			action();
		}
	}
}