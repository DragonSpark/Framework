using System;

namespace DragonSpark.Presentation.Components.Forms
{
	public abstract class ValidatorBase : Radzen.Blazor.ValidatorBase, IDisposable
	{
		// ReSharper disable once FlagArgument
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				messages.Clear();
				base.Dispose();
			}
		}

		public new void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}