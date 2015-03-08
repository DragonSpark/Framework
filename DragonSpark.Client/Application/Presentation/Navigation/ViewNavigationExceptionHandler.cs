using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.IoC;

namespace DragonSpark.Application.Presentation.Navigation
{
	[Singleton( typeof(IViewNavigationExceptionHandler), Priority = Priority.Lowest )]
	public class ViewNavigationExceptionHandler : ViewObject, IViewNavigationExceptionHandler
	{
		readonly IDictionary<Type, Uri> items;

		public ViewNavigationExceptionHandler( IDictionary<Type, Uri> items )
		{
			this.items = items;
		}

		public Uri Handle( Exception e, Uri uri )
		{
			Uri result = null;
			e.GetType().GetHierarchy().FirstOrDefault( x => items.TryGetValue( x, out result ) );
			return result;
		}
	}
}