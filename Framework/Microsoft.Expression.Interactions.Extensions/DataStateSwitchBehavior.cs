// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Markup;
using Microsoft.Expression.Interactions.Extensions.DataHelpers;

namespace Microsoft.Expression.Interactions.Extensions
{
	/// <summary>
	///     Switches between multiple visual states depending on a condition.
	/// </summary>
	[ContentProperty( "States" )]
	public class DataStateSwitchBehavior : Behavior<FrameworkElement>
	{
		public static readonly DependencyProperty StatesProperty = DependencyProperty.Register( "States", typeof(List<DataStateSwitchCase>), typeof(DataStateSwitchBehavior), new PropertyMetadata( null ) );
		readonly BindingListener listener;

		public Binding Binding
		{
			get { return listener.Binding; }
			set { listener.Binding = value; }
		}

		public List<DataStateSwitchCase> States
		{
			get { return (List<DataStateSwitchCase>)GetValue( StatesProperty ); }
			set { SetValue( StatesProperty, value ); }
		}

		static DataStateSwitchBehavior()
		{
		}

		public DataStateSwitchBehavior()
		{
			listener = new BindingListener( HandleBindingValueChanged );
			States = new List<DataStateSwitchCase>();
		}

		static void HandleBindingChanged( DependencyObject sender, DependencyPropertyChangedEventArgs e )
		{
			( (DataStateSwitchBehavior)sender ).OnBindingChanged( e );
		}

		protected virtual void OnBindingChanged( DependencyPropertyChangedEventArgs e )
		{
			listener.Binding = (Binding)e.NewValue;
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
			foreach ( var dataStateSwitchCase in States )
			{
				if ( dataStateSwitchCase.IsValid( listener.Value ) )
				{
					var targetElement = GoToStateBase.FindTargetElement( AssociatedObject );
					if ( targetElement == null )
					{
						break;
					}
					var success = GoToStateBase.GoToState( targetElement, dataStateSwitchCase.State, true );
					break;
				}
			}
		}
	}

	/// <summary>
	///     Represents a case to check if the visual state should be activated.
	/// </summary>
	public class DataStateSwitchCase
	{
		/// <summary>
		///     The value to be compared against
		/// </summary>
		public object Value { get; set; }

		/// <summary>
		///     The name of the state to be activated if the value is true
		/// </summary>
		public string State { get; set; }

		/// <summary>
		///     Compares this value to the target value to determine if the
		///     state should be activated.
		/// </summary>
		/// <param name="targetValue"></param>
		/// <returns></returns>
		public bool IsValid( object targetValue )
		{
			if ( targetValue == null || Value == null )
			{
				return Equals( targetValue, Value );
			}

			var convertedValue = ConverterHelper.ConvertToType( Value, targetValue.GetType() );
			return Equals( targetValue, ConverterHelper.ConvertToType( Value, targetValue.GetType() ) );
		}
	}
}
