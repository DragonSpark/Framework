using System;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.ComponentModel
{
	public static class ViewModelExtensions
	{
		public static ViewModel<TObject> ToModel<TObject>( this TObject target, Action<object> action = null ) where TObject : class
		{
			var result = new ViewModel<TObject>( target );
			action.NotNull( x => x( result ) );
			return result;
		}
	}
}