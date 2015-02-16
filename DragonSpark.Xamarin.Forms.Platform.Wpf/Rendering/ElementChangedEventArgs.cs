using System;
using Xamarin.Forms;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering
{
	public class ElementChangedEventArgs<TElement> : EventArgs where TElement : Element
	{
		public TElement OldElement
		{
			get;
			private set;
		}
		public TElement NewElement
		{
			get;
			private set;
		}
		public ElementChangedEventArgs(TElement oldElement, TElement newElement)
		{
			this.OldElement = oldElement;
			this.NewElement = newElement;
		}
	}
}
