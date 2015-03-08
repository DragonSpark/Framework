using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;
using DragonSpark.Objects;

namespace DragonSpark.Application.Presentation.Interaction
{
    public class DataFormCompensations : Behavior<DataForm>
	{
		public event EventHandler Submitted = delegate { } , Canceled = delegate { }, IsEditingChanged = delegate {};

		readonly ObservableCollection<DataField> source = new ObservableCollection<DataField>(); 

		protected override void OnAttached()
		{
			AssociatedObject.Loaded += AssociatedObjectOnLoaded;
			AssociatedObject.Unloaded += AssociatedObjectOnUnloaded;

			base.OnAttached();
		}

		void AssociatedObjectOnLoaded( object sender, RoutedEventArgs routedEventArgs )
		{
			AssociatedObject.AddingNewItem += AssociatedObjectAddingNewItem;
			AssociatedObject.IsEnabledChanged += AssociatedObjectIsEnabledChanged;
			AssociatedObject.EditEnded += AssociatedObjectEditEnded;
			AssociatedObject.BeginningEdit += AssociatedObjectBeginningEdit;
			AssociatedObject.KeyUp += AssociatedObjectKeyUp;
			AssociatedObject.GotFocus += AssociatedObjectOnGotFocus;
			AssociatedObject.AutoGeneratingField += AssociatedObjectOnAutoGeneratingField;

			if ( !AssociatedObject.AutoEdit && AutoEdit )
			{
				Threading.Application.Start( () =>
				{
					AssociatedObject.BeginEdit();
					AssociatedObject.CurrentItem.As<IModifiable>( x => x.MarkAsModified() );
				} );
			}

		}

		void AssociatedObjectOnUnloaded( object sender, RoutedEventArgs routedEventArgs )
		{
			AssociatedObject.AddingNewItem -= AssociatedObjectAddingNewItem;
			AssociatedObject.IsEnabledChanged -= AssociatedObjectIsEnabledChanged;
			AssociatedObject.EditEnded -= AssociatedObjectEditEnded;
			AssociatedObject.BeginningEdit -= AssociatedObjectBeginningEdit;
			AssociatedObject.KeyUp -= AssociatedObjectKeyUp;
			AssociatedObject.GotFocus -= AssociatedObjectOnGotFocus;
			AssociatedObject.AutoGeneratingField -= AssociatedObjectOnAutoGeneratingField;
		}

		void AssociatedObjectOnAutoGeneratingField( object sender, DataFormAutoGeneratingFieldEventArgs args )
		{
			source.Add( args.Field );
		}

		void AssociatedObjectOnGotFocus( object sender, RoutedEventArgs routedEventArgs )
		{
			var edit = !AutoEdit && EnableCancel && !GetIsEditing( AssociatedObject ) && routedEventArgs.OriginalSource.AsTo<FrameworkElement,bool>( x => x.GetParentOfType<DataField>() != null );
			edit.IsTrue( () => AssociatedObject.BeginEdit() );
		}

		public bool AutoEdit
		{
			get { return GetValue( AutoEditProperty ).To<bool>(); }
			set { SetValue( AutoEditProperty, value ); }
		}	public static readonly DependencyProperty AutoEditProperty = DependencyProperty.Register( "AutoEdit", typeof(bool), typeof(DataFormCompensations), null );

		public bool EnableCancel
		{
			get { return GetValue( EnableCancelProperty ).To<bool>(); }
			set { SetValue( EnableCancelProperty, value ); }
		}	public static readonly DependencyProperty EnableCancelProperty = DependencyProperty.Register( "EnableCancel", typeof(bool), typeof(DataFormCompensations), new PropertyMetadata( true ) );
		
		void AssociatedObjectAddingNewItem(object sender, DataFormAddingNewItemEventArgs e)
		{
			ApplyEdit( true );
			Dispatcher.BeginInvoke( () => AssociatedObject.CurrentItem.WithDefaults() );
		}

		void ApplyEdit( bool on )
		{
			var notify = GetIsEditing( AssociatedObject ) != on;
			SetIsEditing( AssociatedObject, on );
			notify.IsTrue( () => IsEditingChanged( this, EventArgs.Empty ) );
		}

    	static void AssociatedObjectKeyUp(object sender, KeyEventArgs e)
		{
			switch ( e.Key )
			{
				case Key.Escape:
					System.Windows.Application.Current.RootVisual.As<Control>( x => x.Focus() );
					e.Handled = true;
					break;
			}
		}

    	/*BindingExpression ButtonsVisibility { get; set; }*/

		void AssociatedObjectBeginningEdit(object sender, CancelEventArgs e)
		{
			source.Clear();
			ApplyEdit( true );

			/*// Kinda ugly...:
			ButtonsVisibility = AssociatedObject.GetBindingExpression( DataForm.CommandButtonsVisibilityProperty );
			AssociatedObject.CommandButtonsVisibility &= ~ ( DataFormCommandButtonsVisibility.Add | DataFormCommandButtonsVisibility.Delete );*/
		}

		public static readonly DependencyProperty IsEditingProperty =
			DependencyProperty.RegisterAttached( "IsEditing", typeof(bool), typeof(DataFormCompensations),
			                                     new PropertyMetadata( OnIsEditingPropertyChanged ) );

		static void OnIsEditingPropertyChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Targets DataForms explicitly." )]
		public static bool GetIsEditing( DataForm element )
		{
			return (bool)element.GetValue( IsEditingProperty );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Targets DataForms explicitly." )]
		public static void SetIsEditing( DataForm element, bool value )
		{
			element.SetValue( IsEditingProperty, value );
		}

		void AssociatedObjectEditEnded( object sender, DataFormEditEndedEventArgs e )
		{
			ApplyEdit( false );
			switch ( e.EditAction )
			{
				case DataFormEditAction.Commit:
					Submitted( this, EventArgs.Empty );
					break;
				case DataFormEditAction.Cancel:
					Canceled( this, EventArgs.Empty );
					break;
			}

			var update = !AutoEdit && !AssociatedObject.AutoCommit;
			update.IsTrue( () => source.Select( x => x.Content.AsTo<ContentControl,object>( y => y.Content ) ).OfType<IRevertibleChangeTracking>().Distinct().Apply( x =>
			{
				var action = e.EditAction == DataFormEditAction.Commit ? (Action)x.AcceptChanges : x.RejectChanges;
				action();
			} ) );

			AssociatedObject.IsItemChanged.IsTrue( () => AssociatedObject.SetValue( DataForm.IsItemChangedProperty, false ) );
			/*ButtonsVisibility.NotNull( x =>
			{
				AssociatedObject.SetBinding( DataForm.CommandButtonsVisibilityProperty, x.ParentBinding );
				ButtonsVisibility = null;
			});*/
		}

		void AssociatedObjectIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			VisualStateManager.GoToState( AssociatedObject, AssociatedObject.IsEnabled ? "Normal" : "Disabled", true );
		}

		public int FocusedFieldIndex
		{
			get { return GetValue( FocusedFieldIndexProperty ).To<int>(); }
			set { SetValue( FocusedFieldIndexProperty, value ); }
		}	public static readonly DependencyProperty FocusedFieldIndexProperty = DependencyProperty.Register( "FocusedFieldIndex", typeof(int), typeof(DataFormCompensations), new PropertyMetadata( -1 ) );

		protected override void OnDetaching()
		{
			AssociatedObject.Loaded -= AssociatedObjectOnLoaded;
			AssociatedObject.Unloaded -= AssociatedObjectOnUnloaded;

			base.OnDetaching();
		}
	}
}