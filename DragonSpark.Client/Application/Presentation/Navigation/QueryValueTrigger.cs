using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using DragonSpark.Extensions;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Presentation.Navigation
{
	public class QueryValueTrigger : TriggerBase<Page>
	{
		public string ParameterName
		{
			get { return GetValue( ParameterNameProperty ).To<string>(); }
			set { SetValue( ParameterNameProperty, value ); }
		}	public static readonly DependencyProperty ParameterNameProperty = DependencyProperty.Register( "ParameterName", typeof(string), typeof(QueryValueTrigger), null );

		public object TargetValue
		{
			get { return GetValue( TargetValueProperty ).To<object>(); }
			set { SetValue( TargetValueProperty, value ); }
		}	public static readonly DependencyProperty TargetValueProperty = DependencyProperty.Register( "TargetValue", typeof(object), typeof(QueryValueTrigger), null );

		public Type TargetValueType
		{
			get { return GetValue( TargetValueTypeProperty ).To<Type>(); }
			set { SetValue( TargetValueTypeProperty, value ); }
		}	public static readonly DependencyProperty TargetValueTypeProperty = DependencyProperty.Register( "TargetValueType", typeof(Type), typeof(QueryValueTrigger), null );

		protected override void OnAttached()
		{
			AssociatedObject.Loaded += AssociatedObjectOnLoaded;
			base.OnAttached();
		}

		protected override void OnDetaching()
		{
			AssociatedObject.Loaded -= AssociatedObjectOnLoaded;
			base.OnDetaching();
		}

		void AssociatedObjectOnLoaded( object sender, RoutedEventArgs routedEventArgs )
		{
			AssociatedObject.Parent.NotNull( x =>
			{
				var dictionary = AssociatedObject.NavigationContext.QueryString;
				var invoke = dictionary.ContainsKey( ParameterName ) && Equals( dictionary[ ParameterName ].ConvertTo( TargetValueType ), TargetValue.ConvertTo( TargetValueType ) );
				invoke.IsTrue( () => InvokeActions( null ) );
			} );
		}
	}
}