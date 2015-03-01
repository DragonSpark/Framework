using System;

namespace DragonSpark.Application.Presentation.ComponentModel
{
	public class ActiveContext : IDisposable
	{
		readonly IActiveContext target;

		public ActiveContext( IActiveContext target )
		{
			this.target = target;
			this.target.IsActive = true;
		}

		public void Dispose() 
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing )
		{
			target.IsActive = false;
		}
	}
}