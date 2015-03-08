using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Entity.Interactivity
{
	public class DataFieldBinding : Behavior<FrameworkElement>, IActiveContext
	{
		public DataFieldBinding()
		{}

		DataForm Form { get; set; }

		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.Loaded += AssociatedObjectOnLoaded;
			AssociatedObject.Unloaded += AssociatedObjectOnUnloaded;
		}

		void AssociatedObjectOnLoaded( object sender, RoutedEventArgs routedEventArgs )
		{
			Form = AssociatedObject.GetParentOfType<DataForm>();
			Form.EditEnding += EnsureEdit;
			Form.CurrentItemChanged += FormOnCurrentItemChanged;
			Update( Form );
		}

		void AssociatedObjectOnUnloaded( object sender, RoutedEventArgs routedEventArgs )
		{
			Form.NotNull( x =>
			{
				x.EditEnding -= EnsureEdit;
				x.CurrentItemChanged -= FormOnCurrentItemChanged;
			} );
			
			Form = null;

			ClearValue( BindingValueProperty );
		}

	    void FormOnCurrentItemChanged( object sender, EventArgs eventArgs )
		{
			sender.As<DataForm>( Update );
		}

		void Update( DataForm form )
		{
			Owner = form.CurrentItem;
			var field = AssociatedObject.GetParentOfType<DataField>();
			PropertyInfo = form.CurrentItem.Transform( y => y.GetType().GetProperty( field.PropertyPath ) );

			using ( new ActiveContext( this ) )
			{
				Value = PropertyInfo.Transform( x => x.GetValue( Owner, null ) );
			}

			var binding = new Binding( field.PropertyPath ) { Source = Owner, Mode = BindingMode.TwoWay };
			binding.NotNull( y => BindingOperations.SetBinding( this, BindingValueProperty, y ) );
		}

		static void EnsureEdit( object sender, DataFormEditEndingEventArgs e )
		{
			e.Cancel |= FocusManager.GetFocusedElement() == null;
		}

		public object Owner
		{
			get { return GetValue( OwnerProperty ).To<object>(); }
			private set { SetValue( OwnerProperty, value ); }
		}	public static readonly DependencyProperty OwnerProperty = DependencyProperty.Register( "Owner", typeof(object), typeof(DataFieldBinding), null );

		public PropertyInfo PropertyInfo
		{
			get { return GetValue( PropertyInfoProperty ).To<PropertyInfo>(); }
			private set { SetValue( PropertyInfoProperty, value ); }
		}	public static readonly DependencyProperty PropertyInfoProperty = DependencyProperty.Register( "PropertyInfo", typeof(PropertyInfo), typeof(DataFieldBinding), null );

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Used for binding purposes... for purposes of binding a model with its value." )]
		public object Value
		{
			get { return GetValue( ValueProperty ).To<object>(); }
			set { SetValue( ValueProperty, value ); }
		}	public static readonly DependencyProperty ValueProperty = DependencyProperty.Register( "Value", typeof(object), typeof(DataFieldBinding), new PropertyMetadata( OnValueChanged ) );

		static void OnValueChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			d.As<DataFieldBinding>( x =>
			{
				if ( !x.IsActive() && x.BindingValue != e.NewValue )
				{
					x.BindingValue = e.NewValue;
				}
			} );
		}

		object BindingValue
		{
			get { return GetValue( BindingValueProperty ).To<object>(); }
			set { SetValue( BindingValueProperty, value ); }
		}	static readonly DependencyProperty BindingValueProperty = DependencyProperty.Register( "BindingValue", typeof(object), typeof(DataFieldBinding), new PropertyMetadata( OnPropertyChangedCallback ) );

		static void OnPropertyChangedCallback( DependencyObject sender, DependencyPropertyChangedEventArgs args )
		{
			sender.As<DataFieldBinding>( x =>
			{
				if ( x.Value != args.NewValue )
				{
					x.Value = args.NewValue;
				}
			});
		}

		bool IActiveContext.IsActive { get; set; }
	}


}