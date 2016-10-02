using DragonSpark.Runtime;
using System;

namespace DragonSpark.Testing.Framework.Runtime
{
	public sealed class TaskContext : DisposableBase
	{
		readonly Action<Identifier> complete;

		internal TaskContext( Identifier origin, Action<Identifier> complete )
		{
			this.complete = complete;
			Origin = origin;
		}

		public Identifier Origin { get; }

		protected override void OnDispose( bool disposing )
		{
			base.OnDispose( disposing );
			if ( disposing )
			{
				complete( Origin );
			}
		}
	}
}