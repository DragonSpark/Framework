using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using DragonSpark.Extensions;
using Expression.Samples.Interactivity;

namespace DragonSpark.Application.Presentation.Interaction
{
	/*public class PropertyChangedTrigger : Expression.Samples.Interactivity.PropertyChangedTrigger
	{
		protected override void OnBindingChanged(DependencyPropertyChangedEventArgs e)
		{
			Dispatcher.BeginInvoke( () => Invoke( e ) );
		}

		void Invoke( DependencyPropertyChangedEventArgs e )
		{
			base.OnBindingChanged(e);
		}
	}*/

	[DefaultTrigger(typeof(ButtonBase), typeof(System.Windows.Interactivity.EventTrigger), new object[] { "Click" }),
	 DefaultTrigger(typeof(UIElement), typeof(System.Windows.Interactivity.EventTrigger), new object[] { "MouseLeftButtonDown" })]
	public class GoToState : GoToStateBase
	{
		/// <summary>
		/// Does the state transition.
		/// </summary>
		/// <param name="parameter"></param>
		protected override void Invoke(object parameter)
		{
			var success = Target.AsTo<Control,bool>( x => Microsoft.Expression.Interactivity.Core.ExtendedVisualStateManager.GoToElementState( x, StateName, UseTransitions ) || VisualStateManager.GoToState( x, StateName, UseTransitions ) );
			success.IsFalse( () =>
			{
				var group = ResolveGroups( Target ).FirstOrDefault( x => x.States.Cast<System.Windows.VisualState>().Any( y => y.Name == StateName ) );
				var index = group.Transform( x => x.States.Cast<System.Windows.VisualState>().FirstOrDefault( y => y.Name == StateName ).Transform( z => new int?( x.States.IndexOf( z ) ) ) );
				index.NotNull( x => GoToState( x.Value, UseTransitions ) );
			} );
		}

		static IEnumerable<VisualStateGroup> ResolveGroups( FrameworkElement element )
		{
			var parent = element;
			while ( parent != null )
			{
				var groups = VisualStateManager.GetVisualStateGroups( parent ).Transform( x => x.Cast<VisualStateGroup>() );
				if ( groups.Any() )
				{
					return groups.ToList();
				}
				parent = parent.Parent as FrameworkElement;
			}
			return Enumerable.Empty<VisualStateGroup>();
		}
		
		public string StateName
		{
			get { return GetValue( StateNameProperty ).To<string>(); }
			set { SetValue( StateNameProperty, value ); }
		}	public static readonly DependencyProperty StateNameProperty = DependencyProperty.Register( "StateName", typeof(string), typeof(GoToState), null );
	}
}