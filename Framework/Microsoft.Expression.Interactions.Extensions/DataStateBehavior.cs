// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;
using Microsoft.Expression.Interactions.Extensions.DataHelpers;

namespace Microsoft.Expression.Interactions.Extensions
{
	/// <summary>
	///     Behavior that activates two visual states based on the condition of a binding.
	/// </summary>
	public class DataStateBehavior : Behavior<FrameworkElement>
	{
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register( "Value", typeof(object), typeof(DataStateBehavior), new PropertyMetadata( null, HandleValueChanged ) );
		public static readonly DependencyProperty TrueStateProperty = DependencyProperty.Register( "TrueState", typeof(string), typeof(DataStateBehavior), new PropertyMetadata( null ) );
		public static readonly DependencyProperty FalseStateProperty = DependencyProperty.Register( "FalseState", typeof(string), typeof(DataStateBehavior), new PropertyMetadata( null ) );
		readonly BindingListener listener;
		bool? isTrue;

		public Binding Binding
		{
			get { return listener.Binding; }
			set { listener.Binding = value; }
		}

		public object Value
		{
			get { return GetValue( ValueProperty ); }
			set { SetValue( ValueProperty, value ); }
		}

		public string FalseState
		{
			get { return (string)GetValue( FalseStateProperty ); }
			set { SetValue( FalseStateProperty, value ); }
		}

		public string TrueState
		{
			get { return (string)GetValue( TrueStateProperty ); }
			set { SetValue( TrueStateProperty, value ); }
		}

		bool? IsTrue
		{
			get { return isTrue; }
			set
			{
				var nullable1 = isTrue;
				var nullable2 = value;
				if ( ( nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : ( nullable1.HasValue != nullable2.HasValue ? 1 : 0 ) ) == 0 )
				{
					return;
				}
				isTrue = value;
				nullable1 = IsTrue;
				if ( nullable1.HasValue )
				{
					UpdateState( GoToStateBase.FindTargetElement( AssociatedObject ) );
				}
			}
		}

		public DataStateBehavior()
		{
			listener = new BindingListener( HandleBindingValueChanged );
		}

		protected virtual void OnBindingChanged( DependencyPropertyChangedEventArgs e )
		{
			listener.Binding = (Binding)e.NewValue;
		}

		static void HandleValueChanged( DependencyObject sender, DependencyPropertyChangedEventArgs e )
		{
			( (DataStateBehavior)sender ).OnValueChanged( e );
		}

		protected virtual void OnValueChanged( DependencyPropertyChangedEventArgs e )
		{
			CheckState();
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			listener.Element = AssociatedObject;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			listener.Element = null;
		}

		void HandleBindingValueChanged( object sender, BindingChangedEventArgs e )
		{
			CheckState();
		}

		void CheckState()
		{
			if ( Value == null || listener.Value == null )
			{
				IsTrue = Equals( listener.Value, Value );
			}
			else
			{
				ConverterHelper.ConvertToType( Value, listener.Value.GetType() );
				IsTrue = Equals( listener.Value, ConverterHelper.ConvertToType( Value, listener.Value.GetType() ) );
			}
		}

		protected virtual void UpdateState( FrameworkElement targetElement )
		{
			if ( targetElement == null )
			{
				IsTrue = new bool?();
			}
			else if ( IsTrue.Value )
			{
				GoToStateBase.GoToState( targetElement, TrueState, true );
			}
			else
			{
				GoToStateBase.GoToState( targetElement, FalseState, true );
			}
		}
	}
}
