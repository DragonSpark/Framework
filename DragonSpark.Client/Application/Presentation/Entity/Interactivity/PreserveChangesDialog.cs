using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Application.Presentation.Interaction;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Entity.Interactivity
{
    public class PreserveChangesDialog : Behavior<DataForm>, IPreserveChangesDialog
	{
		protected override void OnAttached()
		{
			HtmlWindowCloseMonitor.Instance.WindowClosing += InstanceWindowClosing;
			base.OnAttached();
		}

		void InstanceWindowClosing( object sender, HtmlWindowCloseEventArgs e )
		{
			AssociatedObject.CurrentItem.As<IRevertibleChangeTracking>( x =>
			{
				var cancel = Check( x );
				e.Cancel |= cancel;
				e.DialogMessage = cancel ? Message : e.DialogMessage;
			} );
		}

		bool Check( IChangeTracking changeTracking )
		{
			var isEditing = DataFormCompensations.GetIsEditing( AssociatedObject );
			var result = isEditing && ( AssociatedObject.Mode == DataFormMode.AddNew || !Validate() || changeTracking.IsChanged() );
			return result;
		}

		bool Validate()
		{
			var fields = AssociatedObject.GetAllChildren<DataField>();
			var field = fields.Any() ? FocusManager.GetFocusedElement().As<FrameworkElement>().GetParentOfType<DataField>() : null;
			return fields.All( x => x.Validate() ) && field.Transform( y => fields.Contains( y ) && y.Validate(), () => true );
		}

		protected override void OnDetaching()
		{
			HtmlWindowCloseMonitor.Instance.WindowClosing -= InstanceWindowClosing;
			base.OnDetaching();
		}
		
		public string DialogTitle
		{
			get { return GetValue( DialogTitleProperty ).To<string>(); }
			set { SetValue( DialogTitleProperty, value ); }
		}	public static readonly DependencyProperty DialogTitleProperty = DependencyProperty.Register( "DialogTitle", typeof(string), typeof(PreserveChangesDialog), new PropertyMetadata( "Confirm Loss of Pending Changes" ) );

		public string Message
		{
			get { return GetValue( MessageProperty ).To<string>(); }
			set { SetValue( MessageProperty, value ); }
		}	public static readonly DependencyProperty MessageProperty = DependencyProperty.Register( "Message", typeof(string), typeof(PreserveChangesDialog), new PropertyMetadata( "The item you're editing currently has changes.  Continuing will lose these changes.  Continue and lose your current changes?" ) );

		public bool? CanDismissChanges( IRevertibleChangeTracking currentItem )
		{
			var result = Check( currentItem ) ? MessageBox.Show( Message, DialogTitle, MessageBoxButton.OKCancel ) == MessageBoxResult.OK : (bool?)null;
			return result;
		}
	}
}