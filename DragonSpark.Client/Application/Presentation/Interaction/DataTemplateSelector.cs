using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using DragonSpark.Application.Presentation.Controls;
using DragonSpark.Extensions;
using Expression.Samples.Interactivity.DataHelpers;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class DataTemplateSelector : Behavior<ContentControl>
	{
		readonly BindingListener listener;

		public DataTemplateSelector()
		{
			listener = new BindingListener( OnBindingChanged );
		}

		protected override void OnAttached()
		{
			listener.Element = AssociatedObject;
			base.OnAttached();
		}

		void OnBindingChanged( object sender, BindingChangedEventArgs e )
		{
			Refresh();
		}

		void Refresh()
		{
			AssociatedObject.ContentTemplate = listener.Value.Transform( x => x.GetType().Name.Transform( y => AssociatedObject.FindNameExhaustive<DataTemplate>( y, false ) ) );
		}

		public Binding Binding
		{
			get { return listener.Binding; }
			set { listener.Binding = value; }
		}
	}
}