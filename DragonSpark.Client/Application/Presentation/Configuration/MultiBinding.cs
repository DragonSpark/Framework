using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Markup;
using DragonSpark.Application.Presentation.Converters;
using DragonSpark.Extensions;
using Expression.Samples.Interactivity.DataHelpers;

namespace DragonSpark.Application.Presentation.Configuration
{
	/// <summary>
	/// Allows multiple bindings to a single property.
	/// </summary>
	[ContentProperty( "Bindings" )]
	public class MultiBinding : Behavior<FrameworkElement>
	{
		readonly List<BindingListener> listeners = new List<BindingListener>();

		public MultiBinding()
		{
			SetValue( BindingsProperty, new BindingCollection() );
		}

		protected override void OnAttached()
		{
			listeners.AddRange( Bindings.OfType<Binding>().Select( x => new BindingListener( OnBindingChanged ) { Binding = x, Element =  AssociatedObject } ) );

			base.OnAttached();
		}

		void OnBindingChanged( object sender, BindingChangedEventArgs e )
		{
			Value = Converter.Convert( listeners.Select( x => x.Value ).ToArray(), typeof(object), ConverterParameter, CultureInfo.CurrentCulture );
		}

		public object Value
		{
			get { return GetValue( ValueProperty ); }
			private set { SetValue( ValueProperty, value ); }
		}	public static readonly DependencyProperty ValueProperty = DependencyProperty.Register( "Value", typeof(object), typeof(MultiBinding), null );
		
		public IMultiValueConverter Converter
		{
			get { return GetValue( ConverterProperty ).To<IMultiValueConverter>(); }
			set { SetValue( ConverterProperty, value ); }
		}	public static readonly DependencyProperty ConverterProperty = DependencyProperty.Register( "Converter", typeof(IMultiValueConverter), typeof(MultiBinding), null );

		public object ConverterParameter
		{
			get { return GetValue( ConverterParameterProperty ).To<object>(); }
			set { SetValue( ConverterParameterProperty, value ); }
		}	public static readonly DependencyProperty ConverterParameterProperty = DependencyProperty.Register( "ConverterParameter", typeof(object), typeof(MultiBinding), null );

		public BindingCollection Bindings
		{
			get { return GetValue( BindingsProperty ).To<BindingCollection>(); }
			/*private set { SetValue( BindingsProperty, value ); }*/
		}	public static readonly DependencyProperty BindingsProperty = DependencyProperty.Register( "Bindings", typeof(BindingCollection), typeof(MultiBinding), null );

		/*internal void Initialize()
		{
			Children.Clear();
			foreach ( Binding binding in Bindings )
			{
				var target = new BindingTarget();
				target.SetBinding( BindingTarget.ValueProperty, binding );
				target.PropertyChanged += ( sender, args) => Refresh();
				Children.Add( target );
			}
			Refresh();
		}*/
	}
}